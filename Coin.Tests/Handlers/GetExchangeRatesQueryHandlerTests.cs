using Coin.Application.Features.ExchangeRates.Models;
using Coin.Application.Features.ExchangeRates.Queries.GetExchangeRates;
using Coin.Application.Services.Interfaces;
using FluentAssertions;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Coin.Tests.Handlers
{
    public class GetExchangeRatesQueryHandlerTests
    {
        private readonly Mock<ICurrencyService> _currencyServiceMock;
        private readonly GetExchangeRatesQueryHandler _handler;

        public GetExchangeRatesQueryHandlerTests()
        {
            _currencyServiceMock = new Mock<ICurrencyService>();
            _handler = new GetExchangeRatesQueryHandler(_currencyServiceMock.Object);
        }

        [Fact]
        public async Task Handle_ShouldReturnExchangeRateResponse_WhenDataExists()
        {
            // Arrange
            var query = new GetExchangeRatesQuery
            {
                Currency = "USD",
                Date = new DateTime(2024, 12, 1)
            };

            var expectedResponse = new ExchangeRateResponse { Bid = 3.5m, Ask = 3.7m };

            _currencyServiceMock
                .Setup(service => service.GetExchangeRateAsync(query.Currency, query.Date))
                .ReturnsAsync(expectedResponse);

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            result.Should().Be(expectedResponse);
            _currencyServiceMock.Verify(service => service.GetExchangeRateAsync(query.Currency, query.Date), Times.Once);
        }

        [Fact]
        public async Task Handle_ShouldReturnNull_WhenDataDoesNotExist()
        {
            // Arrange
            var query = new GetExchangeRatesQuery
            {
                Currency = "EUR",
                Date = new DateTime(2024, 12, 1)
            };

            _currencyServiceMock
                .Setup(service => service.GetExchangeRateAsync(query.Currency, query.Date))
                .ReturnsAsync((ExchangeRateResponse?)null);

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            result.Should().BeNull();
            _currencyServiceMock.Verify(service => service.GetExchangeRateAsync(query.Currency, query.Date), Times.Once);
        }
    }

}
