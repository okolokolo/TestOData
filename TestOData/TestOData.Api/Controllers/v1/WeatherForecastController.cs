using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RSM.Core.AspNet.Response;
using RSM.Core.Logging.Extensions.Adapters;
using RSM.Core.Logging.Shared;
using System.Threading.Tasks;
using TestOData.Interfaces.Service;
using TestOData.Model;
using TestOData.Service.Exceptions;

namespace TestOData.Api.Controllers.v1
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private const string ReceivedRequest = "Received request.";

        private readonly ILoggerAdapter<WeatherForecastController> _logger;
        private readonly IWeatherService _weatherService;

        public WeatherForecastController(
            ILoggerAdapter<WeatherForecastController> logger,
            IWeatherService weatherService
            )
        {
            _logger = logger;
            _weatherService = weatherService;
        }

        [HttpGet]
        [ProducesResponseType(typeof(WeeklyForecast), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(WeeklyForecast), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Get()
        {
            _logger.LogDebug(ReceivedRequest, new LogItem<WeatherForecastController>());

            try
            {
                var weatherData = await _weatherService.GetWeeklyForecast().ConfigureAwait(false);

                return new OkObjectResult(weatherData);
            }
            catch (NotFoundException ex)
            {
                _logger.LogDebug(ex.Message, new LogItem<WeatherForecastController>());

                return NotFound();
            }
        }
    }
}
