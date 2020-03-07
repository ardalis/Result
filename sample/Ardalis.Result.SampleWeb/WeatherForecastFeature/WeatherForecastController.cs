using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Ardalis.Sample.Core;
using Ardalis.Sample.Core.DTOs;
using Ardalis.Sample.Core.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
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

        [HttpPost]
        [ConvertResultActionFilter]
        public ActionResult<IEnumerable<WeatherForecast>> GetForecast([FromBody]ForecastRequestDto model)
        {
            var result = _weatherService.GetForecast(model);
            if (result.Status == ResultStatus.NotFound) return NotFound();
            if (result.Status == ResultStatus.Error) return new StatusCodeResult(StatusCodes.Status500InternalServerError);
            if (result.Status == ResultStatus.Invalid)
            {
                foreach (var error in result.ValidationErrors)
                {
                    ModelState.AddModelError(error.Key, error.Value);
                }
                return BadRequest(ModelState);
            }

            return Ok(result.Value);

            // TODO: Write a filter or helper so we can make this one line of code
            // Either return _weatherService.GetForecast(model); and use filter
            // or
            // return GetActionResult(_weatherService.GetForecast(model)); using a helper
        }
    }

    // Need a filter that can translate between Ok(Result<Foo>) into one of NotFound, BadRequest, or Ok(foo)
    // I don't remember which one is best for that - I think it's a ResultFilter
    //public class ResultResultFilter : ResultFilterAttribute
    //{
    //    public override async Task OnResultExecutionAsync(ResultExecutingContext context, ResultExecutionDelegate next)
    //    {
    //        base.OnResultExecutionAsync(context, next);

    //        // how do I get the current ActionResult being returned? I think I have to cast it.
    //        if(context.Result is ActionResult<Result<IEnumerable<WeatherForecast>>>)
    //        {

    //        }

    //        // how to set the result
    //        context.Result = new NotFoundResult();
    //    }
    //}

    public class Helpers
    {
        // https://stackoverflow.com/a/982540/13729 Jon Skeet
        public static bool IsInstanceOfGenericType(Type genericType, object instance)
        {
            Type type = instance.GetType();
            while (type != null)
            {
                if (type.IsGenericType &&
                    type.GetGenericTypeDefinition() == genericType)
                {
                    return true;
                }
                type = type.BaseType;
            }
            return false;
        }
    }

    public class ConvertResultActionFilterAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuted(ActionExecutedContext context)
        {
            var objectResult = context.Result as ObjectResult;

            // not sure I want to leave this but let's keep it for now
            if (objectResult?.Value == null)
            {
                context.Result = new NotFoundResult();
                return;
            }

            bool isResult = Helpers.IsInstanceOfGenericType(typeof(Result<>), objectResult.Value);
            var result = objectResult.Value as Result<IEnumerable<WeatherForecast>>;

            if(result.Errors.Any())
            {
                throw new Exception("Errors occurred.");
            }
        }
    }
}
