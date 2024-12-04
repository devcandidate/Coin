using Coin.Application.Features.ExchangeRates.Models;
using Coin.Application.Services.Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Coin.Application.Features.ExchangeRates.Queries.GetExchangeRates
{
    public class GetExchangeRatesQueryHandler(ICurrencyService currencyService) : IRequestHandler<GetExchangeRatesQuery, ExchangeRateResponse?>
    {
        private readonly ICurrencyService _currencyService = currencyService;

        public async Task<ExchangeRateResponse?> Handle(GetExchangeRatesQuery request, CancellationToken cancellationToken)
        {
            return await _currencyService.GetExchangeRateAsync(request.Currency, request.Date);
        }
    }
}
