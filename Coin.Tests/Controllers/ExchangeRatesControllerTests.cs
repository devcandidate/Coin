using Coin.Api.Controllers;
using Coin.Application.Features.ExchangeRates.Models;
using Coin.Application.Features.ExchangeRates.Queries.GetExchangeRates;
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
    public class ExchangeRatesControllerTests
    {
        private readonly Mock<IMediator> _mediatorMock;
        private readonly ExchangeRatesController _controller;

        public ExchangeRatesControllerTests()
        {
            _mediatorMock = new Mock<IMediator>();
            _controller = new ExchangeRatesController(_mediatorMock.Object, Mock.Of<IConfiguration>());
        }

        [Fact]
        public async Task GetExchangeRates_ShouldReturnOk_WhenResultIsNotNull()
        {
            // Arrange
            var expectedResponse = new ExchangeRateResponse
            {
                Bid = 1.2m,
                Ask = 1.3m
            };

            // Mock IMediator to return the expected response
            _mediatorMock.Setup(m => m.Send(It.IsAny<GetExchangeRatesQuery>(), It.IsAny<CancellationToken>()))
                         .ReturnsAsync(expectedResponse);

            // Act
            var result = await _controller.GetExchangeRates("USD", new DateTime(2022, 02, 17));

            // Assert
            // Verify that the result is OkObjectResult and contains expectedResponse
            var okResult = result as OkObjectResult;
            okResult.Should().NotBeNull();
            okResult!.Value.Should().BeEquivalentTo(expectedResponse);
        }

        [Fact]
        public async Task GetExchangeRates_ShouldReturnNotFound_WhenResultIsNull()
        {
            // Arrange
            _mediatorMock.Setup(m => m.Send(It.IsAny<GetExchangeRatesQuery>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((ExchangeRateResponse?)null);


            // Act
            var result = await _controller.GetExchangeRates("USD", new DateTime(2022, 02, 17));

            // Assert
            result.Should().BeOfType<NotFoundObjectResult>();
        }
    }
}
