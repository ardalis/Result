using Ardalis.ApiEndpoints;
using Ardalis.Result.AspNetCore;
using Ardalis.Sample.Core;
using Ardalis.Sample.Core.DTOs;
using Ardalis.Sample.Core.Model;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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
