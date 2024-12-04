using Coin.Infrastructure.Persistance.Entities;
using Coin.Infrastructure.Persistance;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Coin.Application.Services.Interfaces;
using Coin.Infrastructure.ExternalServices.Interfaces;
using Microsoft.EntityFrameworkCore;
using EFCore.BulkExtensions;
using Microsoft.Extensions.Logging;

namespace Coin.Application.Services
{
    public class ExchangeRatesSyncService(ApplicationDbContext context, INbpApiClient apiClient, ILogger<ExchangeRatesSyncService> logger) : IExchangeRatesSyncService
    {
        private readonly INbpApiClient _nbpApiClient = apiClient;
        private readonly ApplicationDbContext _context = context;
        private readonly ILogger<ExchangeRatesSyncService> _logger = logger;
        private const int MaxDaysPerRequest = 93;
        private const string TableId = "C";

        public async Task SyncExchangeRatesForDateRangeAsync(DateTime startDate, DateTime endDate, CancellationToken cancellationToken)
        {
            if (startDate > endDate)
            {
                throw new ArgumentException("Start date must be earlier than or equal to the end date.");
            }

            // Log the start of synchronization
            _logger.LogInformation("Starting synchronization of exchange rates from {StartDate} to {EndDate}.", startDate, endDate);

            // Split the date range into smaller intervals
            var dateRanges = SplitDateRange(startDate, endDate, MaxDaysPerRequest);

            foreach (var range in dateRanges)
            {
                _logger.LogInformation("Fetching data for range: {StartDate} to {EndDate}.", range.StartDate, range.EndDate);

                cancellationToken.ThrowIfCancellationRequested();

                try
                {
                    // Fetch exchange rates for the specified range
                    var rates = await _nbpApiClient.GetExchangeRatesFromTableAsync(TableId, range.StartDate, range.EndDate);

                    cancellationToken.ThrowIfCancellationRequested();

                    if (rates.Count == 0)
                    {
                        _logger.LogWarning("No data found for range: {StartDate} to {EndDate}.", range.StartDate, range.EndDate);
                        continue;
                    }

                    // Prepare records for batch InsertOrUpdate
                    var bulkData = rates.Select(rate => new ExchangeRate
                    {
                        Currency = rate.Currency,
                        Date = rate.Date,
                        Bid = rate.Bid,
                        Ask = rate.Ask,
                        LastModified = DateTime.UtcNow
                    }).ToList();

                    await _context.BulkInsertOrUpdateAsync(bulkData, (BulkConfig)null, null, null, cancellationToken);

                    _logger.LogInformation("Synchronized data for range: {StartDate} to {EndDate}.", range.StartDate, range.EndDate);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "An error occurred while synchronizing data for range: {StartDate} to {EndDate}.", range.StartDate, range.EndDate);
                    throw;
                }
            }

            _logger.LogInformation("Exchange rate synchronization completed successfully for the range {StartDate} to {EndDate}.", startDate, endDate);
        }

        private static List<(DateTime StartDate, DateTime EndDate)> SplitDateRange(DateTime startDate, DateTime endDate, int maxDays)
        {
            var ranges = new List<(DateTime StartDate, DateTime EndDate)>();
            var currentStart = startDate;

            while (currentStart <= endDate)
            {
                var currentEnd = currentStart.AddDays(maxDays - 1);
                if (currentEnd > endDate)
                {
                    currentEnd = endDate;
                }

                ranges.Add((currentStart, currentEnd));
                currentStart = currentEnd.AddDays(1);
            }

            return ranges;
        }
    }
}
