using Coin.Application.Features.ExchangeRates.Models;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Coin.Application.Features.ExchangeRates.Queries.GetExchangeRates
{
    public class GetExchangeRatesQuery : IRequest<ExchangeRateResponse?>
    {
        public required string Currency { get; set; }
        public DateTime Date { get; set; }
    }
}
