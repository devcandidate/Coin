using Coin.Application.Features.ExchangeRates.Models;
using Coin.Application.Services.Interfaces;
using Coin.Infrastructure.ExternalServices.Interfaces;
using Coin.Infrastructure.Persistance;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Coin.Application.Services
{
    public class CurrencyService(ApplicationDbContext context) : ICurrencyService
    {
        private readonly ApplicationDbContext _context = context;
        public async Task<ExchangeRateResponse?> GetExchangeRateAsync(string currency, DateTime date)
        {
            var rate = await _context.ExchangeRates
                                     .FirstOrDefaultAsync(x => x.Currency == currency.ToUpper() && x.Date == date.Date && x.Bid.HasValue && x.Ask.HasValue);

            return rate is null ? null : new ExchangeRateResponse
            {
                Bid = rate.Bid.Value,
                Ask = rate.Ask.Value
            };
        }
    }
}
