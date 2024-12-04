using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Coin.Application.Features.ExchangeRates.Models
{
    public class ExchangeRateResponse
    {
        public decimal Bid { get; set; }
        public decimal Ask { get; set; }
    }
}
