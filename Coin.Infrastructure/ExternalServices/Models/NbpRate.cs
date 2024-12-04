using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Coin.Infrastructure.ExternalServices.Models
{
    public class NbpRate
    {
        public string Currency { get; set; } // Currency name (e.g., "Euro")
        public string Code { get; set; } // Currency code (e.g., "EUR")
        public decimal? Bid { get; set; } // Bid rate (only for table "C")
        public decimal? Ask { get; set; } // Ask rate (only for table "C")
        public decimal? Mid { get; set; } // Mid rate (for tables "A" and "B")
    }
}
