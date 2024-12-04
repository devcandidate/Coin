using Coin.Infrastructure.ExternalServices.Interfaces;
using Coin.Infrastructure.ExternalServices.Models;
using Coin.Infrastructure.Persistance.Entities;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace Coin.Infrastructure.ExternalServices
{
    public class NbpApiClient : INbpApiClient
    {
        private readonly RestClient _client;
        private readonly ILogger<NbpApiClient> _logger;

        public NbpApiClient(IConfiguration configuration, ILogger<NbpApiClient> logger)
        {
            var baseUrl = configuration["NbpApi:BaseUrl"];

            if (string.IsNullOrEmpty(baseUrl))
            {
                throw new ArgumentException("The NBP API base URL is not configured.", nameof(baseUrl));
            }

            _client = new RestClient(baseUrl);
            _logger = logger;
        }

        /// <summary>
        /// Fetches exchange rates from the specified NBP table (A, B, or C) for the given date range.
        /// </summary>
        /// <param name="table">The table type (A, B, or C).</param>
        /// <param name="startDate">The start date of the range.</param>
        /// <param name="endDate">The end date of the range.</param>
        /// <returns>A list of exchange rates.</returns>
        public async Task<List<ExchangeRate>> GetExchangeRatesFromTableAsync(string table, DateTime startDate, DateTime endDate)
        {
            var formattedStartDate = startDate.ToString("yyyy-MM-dd");
            var formattedEndDate = endDate.ToString("yyyy-MM-dd");
            var endpoint = $"api/exchangerates/tables/{table}/{formattedStartDate}/{formattedEndDate}/?format=json";

            _logger.LogInformation("Fetching exchange rates from table {Table} for range {StartDate} to {EndDate}.", table, formattedStartDate, formattedEndDate);

            try
            {
                var request = new RestRequest(endpoint, Method.Get);

                // Perform the API request
                var response = await _client.ExecuteAsync<List<NbpTable>>(request);

                if (!response.IsSuccessful || response.Data == null || response.Data.Count == 0)
                {
                    _logger.LogWarning("No data found for table {Table} from {StartDate} to {EndDate}.", table, formattedStartDate, formattedEndDate);
                    return [];
                }

                // Transform response to ExchangeRate format
                return response.Data
                    .SelectMany(table => table.Rates.Select(rate => new ExchangeRate
                    {
                        Currency = rate.Code,
                        Bid = rate.Bid,
                        Ask = rate.Ask,
                        Mid = rate.Mid,
                        Date = DateTime.Parse(table.EffectiveDate),
                    }))
                    .ToList();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to fetch exchange rates for table {Table} from {StartDate} to {EndDate}.", table, formattedStartDate, formattedEndDate);
                return [];
            }
        }
    }
}
