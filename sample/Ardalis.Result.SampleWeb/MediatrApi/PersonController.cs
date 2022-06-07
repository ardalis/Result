using Ardalis.Result.AspNetCore;
using Ardalis.Result.Sample.Core.Services;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Threading;
using System.Threading.Tasks;

namespace Ardalis.Result.SampleWeb.MediatrApi;

[ApiController]
[Route("mediatr/[controller]")]
public class PersonController : ControllerBase
{
    private readonly IMediator _mediator;

    public PersonController(IMediator mediator)
    {
        _mediator = mediator;
    }

    /// <summary>
    /// This uses a filter to convert an Ardalis.Result return type to an ActionResult.
    /// This filter could be used per controller or globally!
    /// </summary>
    /// <param name="model"></param>
    /// <returns></returns>
    [TranslateResultToActionResult]
    [HttpDelete("Remove/{id}")]
    public Task<Result> RemovePerson(int id)
    {
        // One might try to perform translation from Result<T> to an appropriate IActionResult from within a MediatR pipeline
        // Unfortunately without having Result<T> depend on IActionResult there doesn't appear to be a way to do this, so this
        // example is still using the TranslateResultToActionResult filter.
        return _mediator.Send(new RemovePersonCommand(id));
    }

    public class RemovePersonCommand : IRequest<Result>
    {
        public RemovePersonCommand(int id)
        {
            Id = id;
        }

        public int Id { get; set; }
    }

    public class RemovePersonCommandHandler : IRequestHandler<RemovePersonCommand, Result>
    {
        private readonly PersonService _personService;

        public RemovePersonCommandHandler(PersonService personService)
        {
            _personService = personService;
        }

        public Task<Result> Handle(RemovePersonCommand request, CancellationToken cancellationToken)
        {
            var result = _personService.Remove(request.Id);

            return Task.FromResult(result);
        }
    }
}
