using Ardalis.ApiEndpoints;
using Ardalis.Result.AspNetCore;
using Ardalis.Result.Sample.Core.Services;
using Microsoft.AspNetCore.Mvc;
using System;
using Ardalis.Result.Sample.Core.DTOs;
using Ardalis.Result.Sample.Core.Model;

namespace Ardalis.Result.SampleWeb.PersonFeature;

public class CreatePersonEndpoint : EndpointBaseSync
    .WithRequest<CreatePersonRequestDto>
    .WithActionResult<Person>
{
    private readonly PersonService _personService;

    public CreatePersonEndpoint(PersonService personService)
    {
        _personService = personService;
    }
    
    /// <summary>
    /// This uses an extension method to convert to an ActionResult
    /// </summary>
    /// <returns></returns>
    [HttpPost("/Person/Create/")]
    public override ActionResult<Person> Handle(CreatePersonRequestDto request)
    {
        if (DateTime.Now.Second % 2 == 0) // just so we can show both versions
        {
            // Extension method on ControllerBase
            return this.ToActionResult(_personService.Create(request.FirstName, request.LastName));
        }

        Result<Person> result = _personService.Create(request.FirstName, request.LastName);

        // Extension method on a Result instance (passing in ControllerBase instance)
        return result.ToActionResult(this);
    }
}
