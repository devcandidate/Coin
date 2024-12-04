using Coin.Application.Features.ExchangeRates.Commands.SyncExchangeRates;
using Coin.Application.Features.ExchangeRates.Queries;
using Coin.Application.Services.Interfaces;
using MediatR;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.Threading;

namespace Coin.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SystemController(IMediator mediator, IConfiguration configuration) : BaseAppController(mediator, configuration)
    {
        [HttpGet("SyncExchangeRates")]
        public async Task<IActionResult> SyncExchangeRates(DateTime startDate, DateTime endDate, string? token, CancellationToken cancellationToken)
        {
            if (!IsTokenValid(token))
            {
                return Unauthorized(new
                {
                    Message = "Invalid auth token."
                });
            }

            await _mediator.Send(new SyncExchangeRatesCommand(startDate, endDate), cancellationToken);
            return Ok();
        }
    }
}
