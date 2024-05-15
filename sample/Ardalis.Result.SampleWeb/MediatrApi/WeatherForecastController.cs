using Ardalis.Result.AspNetCore;
using Ardalis.Result.Sample.Core.DTOs;
using Ardalis.Result.Sample.Core.Model;
using Ardalis.Result.Sample.Core.Services;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Threading;
using System.Threading.Tasks;

namespace Ardalis.Result.SampleWeb.MediatrApi;

[ApiController]
[Route("mediatr/[controller]")]
public class WeatherForecastController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly ILogger<WeatherForecastController> _logger;

    public WeatherForecastController(
        IMediator mediator,
        ILogger<WeatherForecastController> logger)
    {
        _mediator = mediator;
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
    public Task<Result<IEnumerable<WeatherForecast>>> CreateForecast([FromBody] NewForecastCommand model)
    {
        // One might try to perform translation from Result<T> to an appropriate IActionResult from within a MediatR pipeline
        // Unfortunately without having Result<T> depend on IActionResult there doesn't appear to be a way to do this, so this
        // example is still using the TranslateResultToActionResult filter.
        return _mediator.Send(model);
    }

    public class NewForecastCommand : IRequest<Result<IEnumerable<WeatherForecast>>>
    {
        [Required]
        public string PostalCode { get; set; } = String.Empty;
    }

    public class NewForecastHandler : IRequestHandler<NewForecastCommand, Result<IEnumerable<WeatherForecast>>>
    {
        private readonly WeatherService _weatherService;

        public NewForecastHandler(WeatherService weatherService)
        {
            _weatherService = weatherService;
        }
        public Task<Result<IEnumerable<WeatherForecast>>> Handle(NewForecastCommand request, CancellationToken cancellationToken)
        {
            return _weatherService.GetForecastAsync(new ForecastRequestDto { PostalCode = request.PostalCode });
        }
    }
}
