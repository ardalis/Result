using Ardalis.Result.AspNetCore;
using Ardalis.Result.Sample.Core.Services;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Threading;
using System.Threading.Tasks;
using Ardalis.Result.Sample.Core.DTOs;
using Ardalis.Result.Sample.Core.Model;

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

    /// <summary>
    /// This uses a filter to convert an Ardalis.Result return type to an ActionResult.
    /// This filter could be used per controller or globally!
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    [TranslateResultToActionResult]
    [HttpPost("Create/")]
    public Task<Result<Person>> CreatePerson(CreatePersonRequestDto request)
    {
        // One might try to perform translation from Result<T> to an appropriate IActionResult from within a MediatR pipeline
        // Unfortunately without having Result<T> depend on IActionResult there doesn't appear to be a way to do this, so this
        // example is still using the TranslateResultToActionResult filter.
        return _mediator.Send(new CreatePersonCommand(request.FirstName, request.LastName));
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
    
    public class CreatePersonCommand : IRequest<Result<Person>>
    {
        public CreatePersonCommand(string firstName, string lastName)
        {
            FirstName = firstName;
            LastName = lastName;
        }

        public string FirstName { get; set; }
        public string LastName { get; set; }
    }

    public class CreatePersonCommandHandler : IRequestHandler<CreatePersonCommand, Result<Person>>
    {
        private readonly PersonService _personService;

        public CreatePersonCommandHandler(PersonService personService)
        {
            _personService = personService;
        }

        public Task<Result<Person>> Handle(CreatePersonCommand request, CancellationToken cancellationToken)
        {
            var result = _personService.Create(request.FirstName, request.LastName);

            return Task.FromResult(result);
        }
    }
}
