using Coin.Infrastructure.Persistance.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Coin.Infrastructure.ExternalServices.Interfaces
{
    public interface INbpApiClient
    {
        Task<List<ExchangeRate>> GetExchangeRatesFromTableAsync(string table, DateTime startDate, DateTime endDate);
    }
}
