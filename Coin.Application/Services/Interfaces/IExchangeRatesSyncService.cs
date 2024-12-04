using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Coin.Application.Services.Interfaces
{
    public interface IExchangeRatesSyncService
    {
        Task SyncExchangeRatesForDateRangeAsync(DateTime startDate, DateTime endDate, CancellationToken cancellationToken);
    }
}
