using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Coin.Infrastructure.ExternalServices.Models
{
    public class NbpTable
    {
        public string Table { get; set; } // Table type: "A", "B", "C"
        public string EffectiveDate { get; set; } // Effective date of the table
        public List<NbpRate> Rates { get; set; } = new List<NbpRate>(); // List of currency exchange rates
    }
}
