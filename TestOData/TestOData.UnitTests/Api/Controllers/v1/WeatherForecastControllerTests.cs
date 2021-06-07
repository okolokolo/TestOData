using Microsoft.AspNetCore.Mvc;
using Moq;
using RSM.Core.Logging.Extensions.Adapters;
using RSM.Core.Logging.Shared;
using System;
using System.Threading.Tasks;
using TestOData.Api.Controllers.v1;
using TestOData.Interfaces.Service;
using TestOData.Model;
using TestOData.Service.Exceptions;
using Xunit;

namespace TestOData.UnitTests.Api.Controllers.v1
{
    public sealed class WeatherForecastControllerTests : IDisposable
    {
        private readonly MockRepository _moq;
        private readonly Mock<ILoggerAdapter<WeatherForecastController>> _mockLogger;
        private readonly Mock<IWeatherService> _mockWeatherService;
        private readonly WeatherForecastController sut;

        public WeatherForecastControllerTests()
        {
            _moq = new MockRepository(MockBehavior.Strict);
            _mockLogger = _moq.Create<ILoggerAdapter<WeatherForecastController>>();
            _mockWeatherService = _moq.Create<IWeatherService>();

            sut = new WeatherForecastController(_mockLogger.Object, _mockWeatherService.Object);
        }

        [Fact]
        public async Task Get_WhenNoWeatherDataFound_ReturnsNotFoundResponse()
        {
            // Arrange
            _mockLogger.Setup(l => l.LogDebug("Received request.", It.IsAny<LogItem<WeatherForecastController>>()));

            var exceptionMessage = "Weekly forecast not found.";
            _mockWeatherService.Setup(s => s.GetWeeklyForecast())
                .ThrowsAsync(new NotFoundException(exceptionMessage));

            _mockLogger.Setup(l => l.LogDebug(exceptionMessage, It.IsAny<LogItem<WeatherForecastController>>()));

            // Act
            var result = await sut.Get().ConfigureAwait(false);

            // Assert
            Assert.Equal(typeof(NotFoundResult), result.GetType());
            Assert.Equal(404, ((NotFoundResult)result).StatusCode);
        }


        [Fact]
        public async Task Get_WhenWeatherDataFound_ReturnsOKResponse()
        {
            // Arrange
            _mockLogger.Setup(l => l.LogDebug("Received request.", It.IsAny<LogItem<WeatherForecastController>>()));

            var forecast = new WeeklyForecast
            {
                WeeklyLowC = 1,
                WeeklyHighC = 2
            };

            _mockWeatherService.Setup(s => s.GetWeeklyForecast())
                .ReturnsAsync(forecast);

            // Act
            var result = await sut.Get().ConfigureAwait(false);

            // Assert
            Assert.Equal(typeof(OkObjectResult), result.GetType());
            Assert.Equal(200, ((OkObjectResult)result).StatusCode);

            var data = (WeeklyForecast)((OkObjectResult)result).Value;

            Assert.Equal(forecast.WeeklyLowC, data.WeeklyLowC);
            Assert.Equal(forecast.WeeklyHighC, data.WeeklyHighC);
        }

        public void Dispose()
        {
            _moq.VerifyAll();
        }
    }
}
