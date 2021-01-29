using Ardalis.Result.Sample.Core.DTOs;
using Ardalis.Result.Sample.Core.Model;
using Microsoft.Extensions.Localization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ardalis.Result.Sample.Core.Services
{
    public class WeatherService
    {
        public WeatherService(IStringLocalizer<WeatherService> stringLocalizer)
        {
            _stringLocalizer = stringLocalizer;
        }
        private static readonly string[] Summaries = new[]
{
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        private IStringLocalizer<WeatherService> _stringLocalizer;

        public Task<Result<IEnumerable<WeatherForecast>>> GetForecastPagedAsync(ForecastRequestDto model, int pageIndex = 0, int pageSize = 10)
        {
            return Task.FromResult(GetForecastPaged(model, pageIndex, pageSize));
        }
        public Result<IEnumerable<WeatherForecast>> GetForecastPaged(ForecastRequestDto model, int pageIndex = 0, int pageSize = 10)
        {
            if (model.PostalCode == "NotFound") return PagedResult<IEnumerable<WeatherForecast>>.NotFound();

            // validate model
            if (model.PostalCode.Length > 10)
            {
                return Result<IEnumerable<WeatherForecast>>.Invalid(new List<ValidationError> {
                    new ValidationError(nameof(model.PostalCode), _stringLocalizer["PostalCode cannot exceed 10 characters."].Value )
                });
            }

            // test value
            if (model.PostalCode == "55555")
            {
                return new Result<IEnumerable<WeatherForecast>>(Enumerable.Range(1, 1)
                    .Select(index =>
                    new WeatherForecast
                    {
                        Date = DateTime.Now,
                        TemperatureC = 0,
                        Summary = Summaries[0]
                    }));
            }

            var rng = new Random();
            return new Result<IEnumerable<WeatherForecast>>(Enumerable.Range(1, 5)
                .Select(index => new WeatherForecast
                {
                    Date = DateTime.Now.AddDays(index),
                    TemperatureC = rng.Next(-20, 55),
                    Summary = Summaries[rng.Next(Summaries.Length)]
                })
            .ToArray());
        }

        public Task<Result<IEnumerable<WeatherForecast>>> GetForecastAsync(ForecastRequestDto model)
        {
            return Task.FromResult(GetForecast(model));
        }


        public Result<IEnumerable<WeatherForecast>> GetForecast(ForecastRequestDto model)
        {
            if (model.PostalCode == "NotFound") return Result<IEnumerable<WeatherForecast>>.NotFound();

            // validate model
            if (model.PostalCode.Length > 10)
            {
                return Result<IEnumerable<WeatherForecast>>.Invalid(new List<ValidationError> {
                    new ValidationError(nameof(model.PostalCode), _stringLocalizer["PostalCode cannot exceed 10 characters."].Value)
                });
            }

            // test value
            if (model.PostalCode == "55555")
            {
                return new Result<IEnumerable<WeatherForecast>>(Enumerable.Range(1, 1)
                    .Select(index =>
                    new WeatherForecast
                    {
                        Date = DateTime.Now,
                        TemperatureC = 0,
                        Summary = Summaries[0]
                    }));
            }

            var rng = new Random();
            return new Result<IEnumerable<WeatherForecast>>(Enumerable.Range(1, 5)
                .Select(index => new WeatherForecast
                {
                    Date = DateTime.Now.AddDays(index),
                    TemperatureC = rng.Next(-20, 55),
                    Summary = Summaries[rng.Next(Summaries.Length)]
                })
            .ToArray());
        }
    }
}
