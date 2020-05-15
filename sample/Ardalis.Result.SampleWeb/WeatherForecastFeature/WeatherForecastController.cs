using System.Collections.Generic;
using Ardalis.Result.AspNetCore;
using Ardalis.Sample.Core;
using Ardalis.Sample.Core.DTOs;
using Ardalis.Sample.Core.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Ardalis.Result.SampleWeb.WeatherForecastFeature
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private readonly WeatherService _weatherService;
        private readonly ILogger<WeatherForecastController> _logger;

        public WeatherForecastController(
            WeatherService weatherService,
            ILogger<WeatherForecastController> logger)
        {
            _weatherService = weatherService;
            _logger = logger;
        }

        [TranslateResultToActionResult]
        [HttpPost]
        public Result<IEnumerable<WeatherForecast>> CreateForecast([FromBody]ForecastRequestDto model)
        {
            return _weatherService.GetForecast(model);
        }

        // TODO: Implement once Nuget package is updated
        //[HttpPost]
        //public Result<IEnumerable<WeatherForecast>> CreateForecast2([FromBody]ForecastRequestDto model)
        //{
        //    return this.ToActionResult<IEnumerable<WeatherForecast>>(_weatherService.GetForecast(model));
        //    return this._weatherService.GetForecast(model);
        //}
    }
}
