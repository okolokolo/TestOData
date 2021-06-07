using System.Threading.Tasks;

using TestOData.Model;

namespace TestOData.Interfaces.Service
{
    public interface IWeatherService
    {
        Task<WeeklyForecast> GetWeeklyForecast();
    }
}
