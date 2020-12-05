[![NuGet](https://img.shields.io/nuget/v/Ardalis.Result.svg?label=Ardalis.Result%20-%20nuget)](https://www.nuget.org/packages/Ardalis.Result) [![NuGet](https://img.shields.io/nuget/dt/Ardalis.Result.svg)](https://www.nuget.org/packages/Ardalis.Result)

[![Ardails.Result.AspNetCore - NuGet](https://img.shields.io/nuget/v/Ardalis.Result.AspNetCore.svg?label=Ardalis.Result.AspNetCore%20-%20nuget)](https://www.nuget.org/packages/Ardalis.Result.AspNetCore) [![NuGet](https://img.shields.io/nuget/dt/Ardalis.Result.AspNetCore.svg)](https://www.nuget.org/packages/Ardalis.Result.AspNetCore)

[![Ardails.Result.FluentValidation - NuGet](https://img.shields.io/nuget/v/Ardalis.Result.FluentValidation.svg?label=Ardalis.Result.FluentValidation%20-%20nuget)](https://www.nuget.org/packages/Ardalis.Result.FluentValidation) [![NuGet](https://img.shields.io/nuget/dt/Ardalis.Result.FluentValidation.svg)](https://www.nuget.org/packages/Ardalis.Result.FluentValidation)

[![.NET Core](https://github.com/ardalis/Result/workflows/.NET%20Core/badge.svg)](https://github.com/ardalis/Result/actions?query=workflow%3A%22.NET+Core%22)

# Result

A result abstraction that can be mapped to HTTP response codes if needed.

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

The standard way to address these concerns is with exceptions. Maybe you throw a different exception for each different failure mode, and the calling code is then required to have multiple catch blocks designed for each type of failure. This makes life painful for the consumer, and results in a lot of exceptions for things that aren't necessarily *exceptional*.

Another approach is to return a `Tuple` of the expected result along with other things, like a status code and additional failure mode metadata. While tuples can be great for individual, flexible responses, they're not as good for having a single, standard, reusable approach to a problem.

The result pattern provides a standard, reusable way to return both success as well as multiple kinds of non-success responses from .NET services in a way that can easily be mapped to API response types. Although the [Ardalis.Result](https://www.nuget.org/packages/Ardalis.Result/) package has no dependencies on ASP.NET Core and can be used from any .NET Core application, the [Ardalis.Result.AspNetCore](https://www.nuget.org/packages/Ardalis.Result.AspNetCore/) companion package includes resources to enhance the use of this pattern within ASP.NET Core web API applications.

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


## Getting Started

If you're building an ASP.NET Core Web API you can simply install the [Ardalis.Result.AspNetCore](https://www.nuget.org/packages/Ardalis.Result.AspNetCore/) package to get started. Then, apply the `[TranslateResultToActionResult]` attribute to any actions or controllers that you want to automatically translate from Result types to ActionResult types.

