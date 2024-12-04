using Coin.Application.Features.ExchangeRates.Queries;
using Coin.Application.Features.ExchangeRates.Queries.GetExchangeRates;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Coin.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ExchangeRatesController(IMediator mediator, IConfiguration configuration) : BaseAppController(mediator, configuration)
    {      
        [HttpGet("{date:datetime}/{currency}/")]
        public async Task<IActionResult> GetExchangeRates(string currency, DateTime date)
        {
            var query = new GetExchangeRatesQuery() { Currency = currency, Date = date };
           
            var result = await _mediator.Send(query);

            if (result is null)
            {
                return NotFound(result); // exception may be slow in current scenerio
            }

            return Ok(result);
        }
    }
}
