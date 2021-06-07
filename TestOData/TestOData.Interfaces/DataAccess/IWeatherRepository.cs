using System.Collections.Generic;
using System.Threading.Tasks;

using TestOData.Model;

namespace TestOData.Interfaces.DataAccess
{
    public interface IWeatherRepository
    {
        Task<IList<WeatherForecast>> GetWeatherData();
    }
}
