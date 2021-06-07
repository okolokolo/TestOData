using Moq;
using RSM.Core.Logging.Extensions.Adapters;
using RSM.Core.Logging.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TestOData.Interfaces.DataAccess;
using TestOData.Interfaces.Service;
using TestOData.Model;
using TestOData.Service.Exceptions;
using TestOData.Service.Services;
using Xunit;

namespace TestOData.UnitTests.Service.Services
{
    public sealed class WeatherServiceTests : IDisposable
    {
        private readonly MockRepository moq;
        private readonly Mock<IWeatherRepository> _mockWeatherRepository;
        private readonly Mock<ILoggerAdapter<WeatherService>> _mockLogger;
        private readonly IWeatherService sut;

        public WeatherServiceTests()
        {
            moq = new MockRepository(MockBehavior.Strict);
            _mockWeatherRepository = moq.Create<IWeatherRepository>();
            _mockLogger = moq.Create<ILoggerAdapter<WeatherService>>();

            sut = new WeatherService(_mockLogger.Object, _mockWeatherRepository.Object);
        }

        [Fact]
        public void GetWeeklyForecast_WhenWeatherDataNotFound_ThrowsNotFoundResult()
        {
            // Arrange
            _mockWeatherRepository.Setup(r => r.GetWeatherData())
                .ReturnsAsync(new List<WeatherForecast>());
            _mockLogger.Setup(l => l.LogError("No weather data found.", It.IsAny<LogItem<WeatherService>>()));

            // Act & Assert
            Assert.ThrowsAsync<NotFoundException>(() => sut.GetWeeklyForecast());
        }

        [Fact]
        public async Task GetWeeklyForecast_WhenWeatherDataFound_ReturnsWeeklyForecast()
        {
            // Arrange
            var weatherData = new List<WeatherForecast>
            {
                new WeatherForecast
                {
                    Date = DateTime.Now,
                    Summary = "Nice",
                    TemperatureC = 20
                },
                new WeatherForecast
                {
                    Date = DateTime.Now.AddDays(1),
                    Summary = "Hot",
                    TemperatureC = 30
                },
                new WeatherForecast
                {
                    Date = DateTime.Now.AddDays(2),
                    Summary = "Chilly",
                    TemperatureC = 10
                },
                new WeatherForecast
                {
                    Date = DateTime.Now.AddDays(3),
                    Summary = "Freezing",
                    TemperatureC = -10
                },
                new WeatherForecast
                {
                    Date = DateTime.Now.AddDays(4),
                    Summary = "Nice",
                    TemperatureC = 21
                },
            };
            _mockWeatherRepository.Setup(r => r.GetWeatherData())
                .ReturnsAsync(weatherData);

            // Act
            var result = await sut.GetWeeklyForecast().ConfigureAwait(false);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(5, result.Forecasts.Count());
            Assert.Equal(30, result.WeeklyHighC);
            Assert.Equal(-10, result.WeeklyLowC);
        }

        public void Dispose()
        {
            moq.VerifyAll();
        }
    }
}
