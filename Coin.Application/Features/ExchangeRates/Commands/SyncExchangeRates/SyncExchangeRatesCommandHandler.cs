using Coin.Application.Services.Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Coin.Application.Features.ExchangeRates.Commands.SyncExchangeRates
{
    public class SyncExchangeRatesCommandHandler(IExchangeRatesSyncService exchangeRatesSyncService) : IRequestHandler<SyncExchangeRatesCommand>
    {
        private readonly IExchangeRatesSyncService _exchangeRatesSyncService = exchangeRatesSyncService;

        public async Task Handle(SyncExchangeRatesCommand request, CancellationToken cancellationToken)
        {
            await _exchangeRatesSyncService.SyncExchangeRatesForDateRangeAsync(request.StartDate, request.EndDate, cancellationToken);
            return;
        }
    }
}
