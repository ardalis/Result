---
layout: default
title: Getting Started
nav_order: 2
has_children: true
---

# Getting Started

## Adding the Package(s)

To get started, you need to add the appropriate package(s) to your project from NuGet.

[![Ardalis.Result - NuGet](https://img.shields.io/nuget/v/Ardalis.Result.svg?label=Ardalis.Result%20-%20nuget)](https://www.nuget.org/packages/Ardalis.Result) [![NuGet](https://img.shields.io/nuget/dt/Ardalis.Result.svg)](https://www.nuget.org/packages/Ardalis.Result) [![Build Status](https://github.com/ardalis/Result/workflows/.NET%20Core/badge.svg)](https://github.com/ardalis/Result/actions?query=workflow%3A%22.NET+Core%22)

[![Ardails.Result.AspNetCore - NuGet](https://img.shields.io/nuget/v/Ardalis.Result.AspNetCore.svg?label=Ardalis.Result.AspNetCore%20-%20nuget)](https://www.nuget.org/packages/Ardalis.Result.AspNetCore) [![NuGet](https://img.shields.io/nuget/dt/Ardalis.Result.AspNetCore.svg)](https://www.nuget.org/packages/Ardalis.Result.AspNetCore)

[![Ardails.Result.FluentValidation - NuGet](https://img.shields.io/nuget/v/Ardalis.Result.FluentValidation.svg?label=Ardalis.Result.FluentValidation%20-%20nuget)](https://www.nuget.org/packages/Ardalis.Result.FluentValidation) [![NuGet](https://img.shields.io/nuget/dt/Ardalis.Result.FluentValidation.svg)](https://www.nuget.org/packages/Ardalis.Result.FluentValidation)

```bash
dotnet add package Ardalis.Result
```

```bash
dotnet add package Ardalis.Result.AspNetCore
```

```bash
dotnet add package Ardalis.Result.FluentValidation
```

The base `Ardalis.Result` package includes all of the functionality and types needed for use in your domain model or business services. It has no dependency on any third party libraries or UI frameworks.

The `Ardalis.Result.AspNetCore` package includes helpers that can be used to map from `Ardalis.Result` types to `ASP.NET Core` `ActionResult` and `IResult` types used by Controller-based APIs and Minimal APIs.

The `Ardalis.Result.FluentValidation` package is used to allow for easy integration with the [`FluentValidation`](https://www.nuget.org/packages/FluentValidation) package and its validation error types.

## Returning a Result

Imagine you have a method that removes a record from your system:

```csharp
public void Remove(int id)
{
    if (!Exists(id))
    {
        throw new InvalidOperationException($"Record with id {id} Not Found");
    }

    // Remove the record
}
```

It's almost certain that the id sent to this method comes from user input. While it's possible to validate that the input is properly formed and in a valid range, it's usually not possible to use standard validation techniques to ensure the id actually corresponds to a record in the data store. Hence, a call must be made by the service to confirm the record exists, and if it doesn't, an exception may be thrown to indicate the unexpected behavior. But is this behavior truly *exceptional*, given that it's a user-generated input? We know that in our HTTP APIs a missing record is represented by a 404 Not Found, which while not successful, is certainly better than a 500 Server Error response. It's also recommended to [avoid using exceptions for control flow expecially in web APIs](https://ardalis.com/avoid-using-exceptions-determine-api-status/), which we would need to do in this case if we wanted to return a 404, not a 500, from an API that was calling this method as part of its implementation.

How would this changed if we used a Result abstraction?

```csharp
public Result Remove(int id)
{
    if (!Exists(id))
    {
        return Result.NotFound($"Record with id {id} Not Found");
    }

    // Remove the record

    return Result.Success();
}
```

With this change, exceptions are no longer being used as a secondary means of communicating results back to the calling code. The calling code can use normal conditionals to determine what the result of the operation was, and craft its own response or additional behavior accordingly. Furthermore, the code is more intention-revealing, as it is quite clear what the result of the operation is at each return point.
