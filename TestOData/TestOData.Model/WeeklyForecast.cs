using System.Collections.Generic;

namespace TestOData.Model
{
    public class WeeklyForecast
    {
        public int WeeklyHighC { get; set; }
        public int WeeklyLowC { get; set; }
        public IEnumerable<WeatherForecast> Forecasts { get; set; }
    }
}
