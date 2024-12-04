using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Coin.Api.Controllers
{
    public class BaseAppController : ControllerBase
    {
        protected readonly IMediator _mediator;
        private readonly IConfiguration _configuration;
        protected BaseAppController(IMediator mediator, IConfiguration configuration)
        {
            _mediator = mediator;
            _configuration = configuration;
        }

        /* For demo */
        protected bool IsTokenValid(string? token)
        {
            var validToken = _configuration["Authorization:ApiToken"];
            return token == validToken;
        }
    }
}
