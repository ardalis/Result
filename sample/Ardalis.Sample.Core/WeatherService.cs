using Ardalis.Result;
using Ardalis.Sample.Core.DTOs;
using Ardalis.Sample.Core.Model;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Ardalis.Sample.Core
{
    public class WeatherService
    {
        private static readonly string[] Summaries = new[]
{
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        public Result<IEnumerable<WeatherForecast>> GetForecast(ForecastRequestDto model)
        {
            if (model.PostalCode == "NotFound") return Result<IEnumerable<WeatherForecast>>.NotFound();

            // validate model
            if (model.PostalCode.Length > 10)
            {
                return Result<IEnumerable<WeatherForecast>>.Invalid(new Dictionary<string, string> { 
                    { "PostalCode", "PostalCode cannot exceed 10 characters." } 
                });
            }

            // let's add a server error if the forecast times out
            // we'll simulate that with a certain input, 12345
            if(model.PostalCode == "12345")
            {
                return Result<IEnumerable<WeatherForecast>>.Error(new[] { "The forecast timed out." });
            }

            var rng = new Random();
            return new Result<IEnumerable<WeatherForecast>>(Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateTime.Now.AddDays(index),
                TemperatureC = rng.Next(-20, 55),
                Summary = Summaries[rng.Next(Summaries.Length)]
            })
            .ToArray());
        }
    }
}
