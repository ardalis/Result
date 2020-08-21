using Ardalis.ApiEndpoints;
using Ardalis.Result.AspNetCore;
using Ardalis.Result.Sample.Core.DTOs;
using Ardalis.Result.Sample.Core.Model;
using Ardalis.Result.Sample.Core.Services;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace Ardalis.Result.SampleWeb.WeatherForecastFeature
{
    public class ForecastEndpoint : BaseEndpoint<ForecastRequestDto, IEnumerable<WeatherForecast>>
    {
        private readonly WeatherService _weatherService;

        public ForecastEndpoint(WeatherService weatherService)
        {
            _weatherService = weatherService;
        }

        /// <summary>
        /// This uses an extension method to convert to an ActionResult
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost("/Forecast/New")]
        public override ActionResult<IEnumerable<WeatherForecast>> Handle(ForecastRequestDto request)
        {
            return this.ToActionResult(_weatherService.GetForecast(request));
        }
    }
}
