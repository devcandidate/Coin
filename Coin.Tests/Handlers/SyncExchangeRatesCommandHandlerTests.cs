using Coin.Application.Features.ExchangeRates.Commands.SyncExchangeRates;
using Coin.Application.Services.Interfaces;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Coin.Tests.Handlers
{
    public class SyncExchangeRatesCommandHandlerTests
    {
        private readonly Mock<IExchangeRatesSyncService> _syncServiceMock;
        private readonly SyncExchangeRatesCommandHandler _handler;

        public SyncExchangeRatesCommandHandlerTests()
        {
            _syncServiceMock = new Mock<IExchangeRatesSyncService>();
            _handler = new SyncExchangeRatesCommandHandler(_syncServiceMock.Object);
        }

        [Fact]
        public async Task Handle_ShouldCallSyncExchangeRatesForDateRangeAsync()
        {
            // Arrange
            var command = new SyncExchangeRatesCommand(new DateTime(2024, 12, 1), new DateTime(2024, 12, 2)){};

            // Act
            await _handler.Handle(command, CancellationToken.None);

            // Assert
            _syncServiceMock.Verify(
                service => service.SyncExchangeRatesForDateRangeAsync(command.StartDate, command.EndDate, CancellationToken.None),
                Times.Once
            );
        }
    }
}
