---
layout: default
title: Overview
nav_order: 1
has_children: false
---

[![Ardalis.Result - NuGet](https://img.shields.io/nuget/v/Ardalis.Result.svg?label=Ardalis.Result%20-%20nuget)](https://www.nuget.org/packages/Ardalis.Result) [![NuGet](https://img.shields.io/nuget/dt/Ardalis.Result.svg)](https://www.nuget.org/packages/Ardalis.Result) [![Build Status](https://github.com/ardalis/Result/workflows/.NET%20Core/badge.svg)](https://github.com/ardalis/Result/actions?query=workflow%3A%22.NET+Core%22)

[![Ardails.Result.AspNetCore - NuGet](https://img.shields.io/nuget/v/Ardalis.Result.AspNetCore.svg?label=Ardalis.Result.AspNetCore%20-%20nuget)](https://www.nuget.org/packages/Ardalis.Result.AspNetCore) [![NuGet](https://img.shields.io/nuget/dt/Ardalis.Result.AspNetCore.svg)](https://www.nuget.org/packages/Ardalis.Result.AspNetCore) &nbsp; [![Ardails.Result.FluentValidation - NuGet](https://img.shields.io/nuget/v/Ardalis.Result.FluentValidation.svg?label=Ardalis.Result.FluentValidation%20-%20nuget)](https://www.nuget.org/packages/Ardalis.Result.FluentValidation) [![NuGet](https://img.shields.io/nuget/dt/Ardalis.Result.FluentValidation.svg)](https://www.nuget.org/packages/Ardalis.Result.FluentValidation)

<a href="https://twitter.com/intent/follow?screen_name=ardalis">
    <img src="https://img.shields.io/twitter/follow/ardalis.svg?label=Follow%20@ardalis" alt="Follow @ardalis" />
</a> &nbsp; <a href="https://twitter.com/intent/follow?screen_name=nimblepros">
    <img src="https://img.shields.io/twitter/follow/nimblepros.svg?label=Follow%20@nimblepros" alt="Follow @nimblepros" />
</a>

# Result

A result abstraction that can be mapped to HTTP response codes if needed.

## Learn More

* [Getting Started With Ardalis.Result](https://blog.nimblepros.com/blogs/getting-started-with-ardalis-result/)
* [Transforming Results With the Map Method](https://blog.nimblepros.com/blogs/transforming-results-with-the-map-method/)
* [Avoid Using Exceptions to Determine API Status Codes and Responses](https://ardalis.com/avoid-using-exceptions-determine-api-status/)

## What Problem Does This Address?

Many methods on service need to return some kind of value. For instance, they may be looking up some data and returning a set of results or a single object. They might be creating something, persisting it, and then returning it. Typically, such methods are implemented like this:

```csharp
public Customer GetCustomer(int customerId)
{
  // more logic
  return customer;
}

public Customer CreateCustomer(string firstName, string lastName)
{
  // more logic
  return customer;
}
```

This works great as long as we're only concerned with the happy path. But what happens if there are multiple failure modes, not all of which make sense to be handled by exceptions?

- What happens if customerId is not found?
- What happens if required `lastName` is not provided?
- What happens if the current user doesn't have permission to create new customers?

The standard way to address these concerns is with exceptions. Maybe you throw a different exception for each different failure mode, and the calling code is then required to have multiple catch blocks designed for each type of failure. This makes life painful for the consumer, and results in a lot of exceptions for things that aren't necessarily *exceptional*. Like this:

```csharp
[HttpGet]
public async Task<ActionResult<CustomerDTO>> GetCustomer(int customerId)
{
  try
  {
    var customer = _repository.GetById(customerId);
    
    var customerDTO = CustomerDTO.MapFrom(customer);
    
    return Ok(customerDTO);
  }
  catch (NullReferenceException ex)
  {
    return NotFound();
  }
  catch (Exception ex)
  {
    return new StatusCodeResult(StatusCodes.Status500InternalServerError);
  }
}
```

Another approach is to return a `Tuple` of the expected result along with other things, like a status code and additional failure mode metadata. While tuples can be great for individual, flexible responses, they're not as good for having a single, standard, reusable approach to a problem.

The Result pattern provides a standard, reusable way to return both success as well as multiple kinds of non-success responses from .NET services in a way that can easily be mapped to API response types. Although the [Ardalis.Result](https://www.nuget.org/packages/Ardalis.Result/) package has no dependencies on ASP.NET Core and can be used from any .NET Core application, the [Ardalis.Result.AspNetCore](https://www.nuget.org/packages/Ardalis.Result.AspNetCore/) companion package includes resources to enhance the use of this pattern within ASP.NET Core web API applications.

## Sample Usage

### Creating a Result

The [sample folder](https://github.com/ardalis/Result/tree/main/sample/Ardalis.Result.SampleWeb) includes some examples of how to use the project. Here are a couple of simple uses.

Imagine the snippet below is defined in a domain service that retrieves WeatherForecasts. When compared to the approach described above, this approach uses a result to handle common failure scenarios like missing data denoted as NotFound and or input validation errors denoted as Invalid. If execution is successful, the result will contain the random data generated by the final return statement.

```csharp
public Result<IEnumerable<WeatherForecast>> GetForecast(ForecastRequestDto model)
{
    if (model.PostalCode == "NotFound") return Result<IEnumerable<WeatherForecast>>.NotFound();

    // validate model
    if (model.PostalCode.Length > 10)
    {
        return Result<IEnumerable<WeatherForecast>>.Invalid(new List<ValidationError> {
            new ValidationError
            {
                Identifier = nameof(model.PostalCode),
                ErrorMessage = "PostalCode cannot exceed 10 characters." 
            }
        });
    }

    var rng = new Random();
    return new Result<IEnumerable<WeatherForecast>>(Enumerable.Range(1, 5)
        .Select(index => new WeatherForecast
        {
            Date = DateTime.Now.AddDays(index),
            TemperatureC = rng.Next(-20, 55),
            Summary = Summaries[rng.Next(Summaries.Length)]
        })
    .ToArray());
}
```

### Translating Results to ActionResults

Continuing with the domain service example from the previous section, it's important to show that the domain service doesn't know about `ActionResult` or other MVC/etc types. But since it is using a `Result<T>` abstraction, it can return results that are easily mapped to HTTP status codes. Note that the method above returns a `Result<IEnumerable<WeatherForecast>` but in some cases, it might need to return an `Invalid` result, or a `NotFound` result. Otherwise, it returns a `Success` result with the actual returned value (just like an API would return an HTTP 200 and the actual result of the API call).

You can apply the `[TranslateResultToActionResult]` attribute to an [API Endpoint](https://github.com/ardalis/ApiEndpoints) (or controller action if you still use those things) and it will automatically translate the `Result<T>` return type of the method to an `ActionResult<T>` appropriately based on the Result type.

```csharp
[TranslateResultToActionResult]
[HttpPost("Create")]
public Result<IEnumerable<WeatherForecast>> CreateForecast([FromBody]ForecastRequestDto model)
{
    return _weatherService.GetForecast(model);
}
```

Alternatively, you can use the `ToActionResult` helper method within an endpoint to achieve the same thing:

```csharp
[HttpPost("/Forecast/New")]
public override ActionResult<IEnumerable<WeatherForecast>> Handle(ForecastRequestDto request)
{
    return this.ToActionResult(_weatherService.GetForecast(request));

    // alternately
    // return _weatherService.GetForecast(request).ToActionResult(this);
}
```

### Translating Results to Minimal API Results

Similar to how the `ToActionResult` extension method translates `Ardalis.Results` to `ActionResults`, the `ToMinimalApiResult` translates results to the new `Microsoft.AspNetCore.Http.Results` `IResult` types in .NET 6+. The following code snippet demonstrates how one might use the domain service that returns a `Result<IEnumerable<WeatherForecast>>` and convert to a `Microsoft.AspNetCore.Http.Results` instance.

```csharp
app.MapPost("/Forecast/New", (ForecastRequestDto request, WeatherService weatherService) =>
{
    return weatherService.GetForecast(request).ToMinimalApiResult();
})
.WithName("NewWeatherForecast");
```

The full Minimal API sample can be found in the [sample folder](./sample/Ardalis.Result.SampleMinimalApi/Program.cs).

### Mapping Results From One Type to Another

A common use case is to map between domain entities to API response types usually represented as DTOs. You can map a result containing a domain entity to a Result containing a DTO by using the `Map` method. The following example calls the method `_weatherService.GetSingleForecast` which returns a `Result<WeatherForecast>` which is then converted to a `Result<WeatherForecastSummaryDto>` by the call to `Map`. Then, the result is converted to an `ActionResult<WeatherForecastSummaryDto>` using the `ToActionResult` helper method.

```csharp
[HttpPost("Summary")]
public ActionResult<WeatherForecastSummaryDto> CreateSummaryForecast([FromBody] ForecastRequestDto model)
{
    return _weatherService.GetSingleForecast(model)
        .Map(wf => new WeatherForecastSummaryDto(wf.Date, wf.Summary))
        .ToActionResult(this);
}
```

## ASP.NET API Metadata

By default, Asp Net Core and API Explorer know nothing about `[TranslateResultToActionResult]` and what it is doing. To reflect `[TranslateResultToActionResult]` behavior in metadata generated by API Explorer (which is then used by tools like Swashbuckle, NSwag etc.), you can use `ResultConvention`:

```csharp
services.AddControllers(mvcOptions => mvcOptions.AddDefaultResultConvention());
```

This will add `[ProducesResponseType]` for every known `ResultStatus` to every endpoint marked with `[TranslateResultToActionResult]`.
To customize ResultConvention behavior, one may use the `AddResultConvention` method:

```csharp
services.AddControllers(mvcOptions => mvcOptions
    .AddResultConvention(resultStatusMap => resultStatusMap
        .AddDefaultMap()
     ));
```

This code is functionally equivalent to the previous example.

From here you can modify the ResultStatus to HttpStatusCode mapping

```csharp
services.AddControllers(mvcOptions => mvcOptions
    .AddResultConvention(resultStatusMap => resultStatusMap
        .AddDefaultMap()
        .For(ResultStatus.Ok, HttpStatusCode.OK, resultStatusOptions => resultStatusOptions
            .For("POST", HttpStatusCode.Created)
            .For("DELETE", HttpStatusCode.NoContent))
        .For(ResultStatus.Error, HttpStatusCode.InternalServerError)
    ));
```

`ResultConvention` will add `[ProducesResponseType]` for every result status configured in `ResultStatusMap`. `AddDefaultMap()` maps every known ResultType, so if you want to exclude certain ResultType from being listed (e.g. your app doesn't have authentication and authorization, and you don't want 401 and 403 to be listed as `SupportedResponseType`) you can do this:

```csharp
services.AddControllers(mvcOptions => mvcOptions
    .AddResultConvention(resultStatusMap => resultStatusMap
        .AddDefaultMap()
        .For(ResultStatus.Ok, HttpStatusCode.OK, resultStatusOptions => resultStatusOptions
            .For("POST", HttpStatusCode.Created)
            .For("DELETE", HttpStatusCode.NoContent))
        .Remove(ResultStatus.Forbidden)
        .Remove(ResultStatus.Unauthorized)
    ));
```

Alternatively, you can specify which (failure) result statuses are expected from a certain endpoint:

```csharp
[TranslateResultToActionResult()]
[ExpectedFailures(ResultStatus.NotFound, ResultStatus.Invalid)]
[HttpDelete("Remove/{id}")]
public Result RemovePerson(int id)
{
    // Method logic
}
```

`!!!` If you list a certain Result status in `ExpectedFailures`, it must be configured in `ResultConvention` on startup.

Another configurable feature is what part of the Result object is returned in case of specific failure:

```csharp
services.AddControllers(mvcOptions => mvcOptions
    .AddResultConvention(resultStatusMap => resultStatusMap
        .AddDefaultMap()
        .For(ResultStatus.Error, HttpStatusCode.BadRequest, resultStatusOptions => resultStatusOptions
            .With((ctrlr, result) => string.Join("\r\n", result.ValidationErrors)))
    ));
```

### Using Results with FluentValidation

We can use Ardalis.Result.FluentValidation on a service with FluentValidation like that:

```csharp
public async Task<Result<BlogCategory>> UpdateAsync(BlogCategory blogCategory)
{
    if (Guid.Empty == blogCategory.BlogCategoryId) return Result<BlogCategory>.NotFound();

    var validator = new BlogCategoryValidator();
    var validation = await validator.ValidateAsync(blogCategory);
    if (!validation.IsValid)
    {
        return Result<BlogCategory>.Invalid(validation.AsErrors());
    }

    var itemToUpdate = (await GetByIdAsync(blogCategory.BlogCategoryId)).Value;
    if (itemToUpdate == null)
    {
        return Result<BlogCategory>.NotFound();
    }

    itemToUpdate.Update(blogCategory.Name, blogCategory.ParentId);

    return Result<BlogCategory>.Success(await _blogCategoryRepository.UpdateAsync(itemToUpdate));
}
```
