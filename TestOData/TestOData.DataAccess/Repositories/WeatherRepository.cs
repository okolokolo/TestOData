using RSM.Core.Logging.Extensions.Adapters;
using RSM.Core.Logging.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TestOData.Interfaces.DataAccess;
using TestOData.Model;

namespace TestOData.DataAccess.Repositories
{
    public class WeatherRepository : IWeatherRepository
    {
        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        private readonly ILoggerAdapter<WeatherRepository> _logger;

        public WeatherRepository
            (
                ILoggerAdapter<WeatherRepository> logger
            )
        {
            _logger = logger;
        }

        public async Task<IList<WeatherForecast>> GetWeatherData()
        {
            var rng = new Random();

            var forecasts = Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateTime.Now.AddDays(index),
                TemperatureC = rng.Next(-20, 55),
                Summary = Summaries[rng.Next(Summaries.Length)],
            }).ToList();

            _logger.LogDebug($"Retrieved {forecasts.Count} forecasts", new LogItem<WeatherRepository>());

            return await Task.FromResult(forecasts.ToList()).ConfigureAwait(false);
        }
    }
}
