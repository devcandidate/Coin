using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Coin.Infrastructure.Persistance.Entities
{
    public class ExchangeRate
    {
        public required string Currency { get; set; }
        public DateTime Date { get; set; }
        public decimal? Bid { get; set; }
        public decimal? Ask { get; set; }
        public decimal? Mid { get; set; }
        public DateTime LastModified { get; set; }
    }
}
