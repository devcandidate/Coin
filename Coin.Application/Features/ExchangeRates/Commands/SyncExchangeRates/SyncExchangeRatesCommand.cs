using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Coin.Application.Features.ExchangeRates.Commands.SyncExchangeRates
{
    public class SyncExchangeRatesCommand(DateTime startDate, DateTime endDate) : IRequest
    {
        public DateTime StartDate { get; } = startDate;
        public DateTime EndDate { get; } = endDate;
    }
}
