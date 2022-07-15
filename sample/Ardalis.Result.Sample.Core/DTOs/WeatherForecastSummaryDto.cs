using Ardalis.Result.Sample.Core.Model;
using System;

namespace Ardalis.Result.Sample.Core.DTOs
{
    public class WeatherForecastSummaryDto
    {
        public DateTime Date { get; set; }
        public string Summary { get; set; }

        public WeatherForecastSummaryDto(DateTime date, string summary)
        {
            Date = date;
            Summary = summary;
        }

        public static WeatherForecastSummaryDto CreateFromModel(WeatherForecast weatherForecast) =>
            new WeatherForecastSummaryDto(weatherForecast.Date, weatherForecast.Summary);
    }
}
