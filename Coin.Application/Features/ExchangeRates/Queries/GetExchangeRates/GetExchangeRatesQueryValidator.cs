using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Coin.Application.Features.ExchangeRates.Queries.GetExchangeRates
{
    public class GetExchangeRatesQueryValidator : AbstractValidator<GetExchangeRatesQuery>
    {
        public GetExchangeRatesQueryValidator()
        {
            RuleFor(x => x.Currency).NotEmpty()
                                    .WithMessage("Currency is required.")
                                    .Matches("^[A-Za-z]{3}$")
                                    .WithMessage("Currency must be a valid ISO 4217 code (e.g., EUR, USD).");


            RuleFor(x => x.Date).NotEmpty()
                                .WithMessage("Date is required.")
                                .LessThanOrEqualTo(DateTime.UtcNow)
                                .WithMessage("Date cannot be in the future.");
        }
    }
}
