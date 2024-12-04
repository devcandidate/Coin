using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Coin.Application.Features.ExchangeRates.Commands.SyncExchangeRates
{
    public class SyncExchangeRatesCommandValidator : AbstractValidator<SyncExchangeRatesCommand>
    {
        public SyncExchangeRatesCommandValidator()
        {
            RuleFor(x => x.StartDate)
                .NotEmpty()
                .WithMessage("Start date is required.")
                .LessThanOrEqualTo(DateTime.UtcNow)
                .WithMessage("Start date cannot be in the future.");

            RuleFor(x => x.EndDate)
                .NotEmpty()
                .WithMessage("End date is required.")
                .LessThanOrEqualTo(DateTime.UtcNow)
                .WithMessage("End date cannot be in the future.");

            RuleFor(x => x)
                .Must(x => x.StartDate <= x.EndDate)
                .WithMessage("Start date must be earlier than or equal to the end date.");
        }
    }
}
