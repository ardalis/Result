---
layout: default
title: Quick Start Guide
parent: Getting Started
nav_order: 1
---

# Getting Started

If you're building an ASP.NET Core Web API you can simply install the [Ardalis.Result.AspNetCore](https://www.nuget.org/packages/Ardalis.Result.AspNetCore/) package to get started. Then, apply the `[TranslateResultToActionResult]` attribute to any actions or controllers that you want to automatically translate from Result types to ActionResult types.

## Minimal API Example

Add the `.ToMinimalApiResult()` extension method to the return statement of minimal APIs:

```csharp
app.MapPost("/Forecast/New", (ForecastRequestDto request, WeatherService weatherService) =>
{
    return weatherService.GetForecast(request).ToMinimalApiResult(); // 👈
});
```

## Controller-based APIs

Add the `[[TranslateResultToActionResult]` attribute to action methods to perform automatic translation. Be sure to change the return type of the method to a `Result` or `Result<T>` type.

```csharp
/// <summary>
/// This uses a filter to convert an Ardalis.Result return type to an ActionResult.
/// This filter could be used per controller or globally!
/// </summary>
/// <returns></returns>
[TranslateResultToActionResult] // 👈
[ExpectedFailures(ResultStatus.NotFound, ResultStatus.Invalid)]
[HttpDelete("Remove/{id}")]
public Result RemovePerson(int id)
{
    return _personService.Remove(id);
}

/// <summary>
/// This uses a filter to convert an Ardalis.Result return type to an ActionResult.
/// This filter could be used per controller or globally!
/// </summary>
/// <returns></returns>
[TranslateResultToActionResult] // 👈
[ExpectedFailures(ResultStatus.NotFound, ResultStatus.Invalid)]
[HttpPost("New/")]
public Result<Person> CreatePerson(CreatePersonRequestDto request)
{
    return _personService.Create(request.FirstName, request.LastName);
}
```

Alternately, you can use an extension method within the action method to translate a result to an `ActionResult<T>`:

```csharp
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
        return this.ToActionResult(_personService.Create(request.FirstName, request.LastName)); // 👈
    }

    Result<Person> result = _personService.Create(request.FirstName, request.LastName);

    // Extension method on a Result instance (passing in ControllerBase instance)
    return result.ToActionResult(this); // 👈
}
```