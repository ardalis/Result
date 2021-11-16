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
        catch (ForecastNotFoundException ex) // avoid using exceptions for control flow
        {
            return NotFound();
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
            _logger.LogInformation($"Starting {nameof(CreateForecast)}");
        }
    }
}
