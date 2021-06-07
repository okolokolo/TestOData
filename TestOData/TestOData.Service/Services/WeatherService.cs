using RSM.Core.Logging.Extensions.Adapters;
using RSM.Core.Logging.Shared;
using System.Linq;
using System.Threading.Tasks;
using TestOData.Interfaces.DataAccess;
using TestOData.Interfaces.Service;
using TestOData.Model;
using TestOData.Service.Exceptions;

namespace TestOData.Service.Services
{
    internal class WeatherService : IWeatherService
    {
        private readonly ILoggerAdapter<WeatherService> _logger;
        private readonly IWeatherRepository _weatherRepository;

        public WeatherService
            (
                ILoggerAdapter<WeatherService> logger,
                IWeatherRepository weatherRepository
            )
        {
            _logger = logger;
            _weatherRepository = weatherRepository;
        }

        public async Task<WeeklyForecast> GetWeeklyForecast()
        {
            var weatherForecasts = await _weatherRepository.GetWeatherData().ConfigureAwait(false);

            if (weatherForecasts.Count == 0)
            {
                _logger.LogError("No weather data found.", new LogItem<WeatherService>());

                throw new NotFoundException("No weather data found.");
            }

            var weeklyForecast = new WeeklyForecast
            {
                Forecasts = weatherForecasts,
                WeeklyHighC = weatherForecasts.Max(f => f.TemperatureC),
                WeeklyLowC = weatherForecasts.Min(f => f.TemperatureC),
            };

            return weeklyForecast;
        }
    }
}
