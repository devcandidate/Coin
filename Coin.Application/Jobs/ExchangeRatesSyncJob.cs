using Coin.Application.Features.ExchangeRates.Commands.SyncExchangeRates;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Coin.Application.Jobs
{
    public class ExchangeRatesSyncJob(IMediator mediator, ILogger<ExchangeRatesSyncJob> logger)
    {
        private readonly IMediator _mediator = mediator;
        private readonly ILogger<ExchangeRatesSyncJob> _logger = logger;

        public async Task SyncExchangeRatesAsync()
        {
            var endDate = DateTime.UtcNow;
            var startDate = endDate.AddDays(-1);

            _logger.LogInformation("Starting exchange rate synchronization for the range {StartDate} to {EndDate}.", startDate, endDate);

            try
            {
                await _mediator.Send(new SyncExchangeRatesCommand(startDate, endDate));

                _logger.LogInformation("Exchange rates synchronized successfully from {StartDate} to {EndDate}.", startDate, endDate);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred during exchange rate synchronization for the range {StartDate} to {EndDate}.", startDate, endDate);
                throw; // Rethrowing to ensure the error is handled by the caller or scheduler.
            }
        }
    }
}
