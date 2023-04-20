using Ardalis.Result.Sample.Core.DTOs;
using Ardalis.Result.Sample.Core.Exceptions;
using Ardalis.Result.Sample.Core.Model;
using Ardalis.Result.Sample.Core.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Ardalis.Result.SampleWeb.BadApproaches;

/// <summary>
/// Why is this "bad"? Because expected negative conditions are not exceptional - they're expected.
/// It's the same reason why Controllers return ActionResults with different options instead of just returning 
/// your object with Ok and otherwise throwing exceptions. MVC is meant to work with different result types, not exceptions.
/// 
/// Can you make this work using an ExceptionFilter? Absolutely. I still prefer using a proper result type in the service itself,
/// rather than using exceptions for control flow.
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class BadWeatherForecastController : ControllerBase
{
    private readonly WeatherServiceWithExceptions _weatherService;
    private readonly ILogger<BadWeatherForecastController> _logger;

    public BadWeatherForecastController(
        WeatherServiceWithExceptions weatherService,
        ILogger<BadWeatherForecastController> logger)
    {
        _weatherService = weatherService;
        _logger = logger;
    }

    /// <summary>
    /// Wrap logging around service call in every action method
    /// Use try-catch and exceptions to handle different return types
    /// See: https://ardalis.com/avoid-using-exceptions-determine-api-status/
    /// </summary>
    /// <param name="model"></param>
    /// <returns></returns>
    [HttpPost("Create")]
    public async Task<ActionResult<IEnumerable<WeatherForecast>>> CreateForecast([FromBody] ForecastRequestDto model)
    {
        _logger.LogInformation($"Starting {nameof(CreateForecast)}");
        try
        {
            var result = await _weatherService.GetForecastAsync(model);
            return Ok(result);
        }
        catch (ForecastNotFoundException) // avoid using exceptions for control flow
        {
            return NotFound();
        }
        catch (ForecastConflictException) // avoid using exceptions for control flow
        {
            return Conflict();
        }
        catch (ForecastRequestInvalidException ex) // avoid using exceptions for control flow
        {
            var dict = new ModelStateDictionary();
            foreach(var key in ex.ValidationErrors.Keys)
            {
                dict.AddModelError(key, ex.ValidationErrors[key]);
            }
            return BadRequest(dict);
        }
        finally
        {
            _logger.LogInformation($"Finished {nameof(CreateForecast)}");
        }
    }
}
