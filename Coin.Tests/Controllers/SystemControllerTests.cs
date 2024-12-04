using Coin.Api.Controllers;
using Coin.Application.Features.ExchangeRates.Commands.SyncExchangeRates;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Coin.Tests.Controllers
{
    public class SystemControllerTests
    {
        private readonly Mock<IMediator> _mediatorMock;
        private readonly SystemController _controller;

        public SystemControllerTests()
        {
            _mediatorMock = new Mock<IMediator>();
            _controller = new SystemController(_mediatorMock.Object, Mock.Of<IConfiguration>());
        }

        [Fact]
        public async Task SyncExchangeRates_ShouldReturnUnauthorized_WhenTokenIsInvalid()
        {
            // Act
            var result = await _controller.SyncExchangeRates(new DateTime(2022, 02, 16), new DateTime(2022, 02, 17), "invalid-token", CancellationToken.None);

            // Assert
            result.Should().BeOfType<UnauthorizedObjectResult>();
        }
    }
}
