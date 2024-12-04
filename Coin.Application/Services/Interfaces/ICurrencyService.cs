using Coin.Application.Features.ExchangeRates.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Coin.Application.Services.Interfaces
{
    public interface ICurrencyService
    {
        Task<ExchangeRateResponse?> GetExchangeRateAsync(string currency, DateTime date);
    }
}
