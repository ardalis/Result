using Ardalis.Result.AspNetCore;
using Ardalis.Result.Sample.Core.Services;
using Microsoft.AspNetCore.Mvc;

namespace Ardalis.Result.SampleWeb.PersonFeature;

[ApiController]
[Route("[controller]")]
public class PersonController : ControllerBase
{
    private readonly PersonService _personService;

    public PersonController(PersonService personService)
    {
        _personService = personService;
    }

    /// <summary>
    /// This uses a filter to convert an Ardalis.Result return type to an ActionResult.
    /// This filter could be used per controller or globally!
    /// </summary>
    /// <param name="model"></param>
    /// <returns></returns>
    [TranslateResultToActionResult]
    [ExpectedFailures(ResultStatus.NotFound, ResultStatus.Invalid)]
    [HttpDelete("Remove/{id}")]
    public Result RemovePerson(int id)
    {
        return _personService.Remove(id);
    }
}
