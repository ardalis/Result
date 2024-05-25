using Ardalis.Result.Sample.Core.DTOs;
using Ardalis.Result.Sample.Core.Exceptions;
using Ardalis.Result.Sample.Core.Model;
using Microsoft.Extensions.Localization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ardalis.Result.Sample.Core.Services;
public class WeatherServiceWithExceptions
{
    public WeatherServiceWithExceptions(IStringLocalizer<WeatherService> stringLocalizer)
    {
        _stringLocalizer = stringLocalizer;
    }

    private static readonly string[] Summaries = new[]
{
        "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
    };

    private IStringLocalizer<WeatherService> _stringLocalizer;

    public Task<IEnumerable<WeatherForecast>> GetForecastAsync(ForecastRequestDto model)
    {
        return Task.FromResult(GetForecast(model));
    }

    public IEnumerable<WeatherForecast> GetForecast(ForecastRequestDto model)
    {
        switch (model.PostalCode)
        {
            case "NotFound":
                throw new ForecastNotFoundException();
            case "Conflict":
                throw new ForecastConflictException();
        }

        // validate model
        if (model.PostalCode.Length > 10)
        {
            throw new ForecastRequestInvalidException(new Dictionary<string, string>()
            {
                { nameof(model.PostalCode), _stringLocalizer["PostalCode cannot exceed 10 characters."].Value }
            });
        }

        // test single result value
        if (model.PostalCode == "55555")
        {
            return new List<WeatherForecast>
            {
                new WeatherForecast
                {
                    Date = DateTime.Now,
                    TemperatureC = 0,
                    Summary = Summaries[0]
                }
            };
        }

        var rng = new Random();
        return new List<WeatherForecast>(Enumerable.Range(1, 5)
            .Select(index => new WeatherForecast
            {
                Date = DateTime.Now.AddDays(index),
                TemperatureC = rng.Next(-20, 55),
                Summary = Summaries[rng.Next(Summaries.Length)]
            })
        .ToArray());
    }
}
