using System.Collections.Generic;
using Ardalis.Result.AspNetCore;
using Ardalis.Sample.Core;
using Ardalis.Sample.Core.DTOs;
using Ardalis.Sample.Core.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Swashbuckle.AspNetCore.Annotations;

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

        /// <summary>
        /// This uses a filter to convert an Ardalis.Result return type to an ActionResult.
        /// This filter could be used per controller or globally!
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [TranslateResultToActionResult]
        [HttpPost("Create")]
        public Result<IEnumerable<WeatherForecast>> CreateForecast([FromBody]ForecastRequestDto model)
        {
            return _weatherService.GetForecast(model);
        }
    }
}
