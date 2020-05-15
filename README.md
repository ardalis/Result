Ardalis.Result: [![NuGet](https://img.shields.io/nuget/v/Ardalis.Result.svg)](https://www.nuget.org/packages/Ardalis.Result)[![NuGet](https://img.shields.io/nuget/dt/Ardalis.Result.svg)](https://www.nuget.org/packages/Ardalis.Result)

Ardails.Result.AspNetCore: [![NuGet](https://img.shields.io/nuget/v/Ardalis.Result.AspNetCore.svg)](https://www.nuget.org/packages/Ardalis.Result.AspNetCore)[![NuGet](https://img.shields.io/nuget/dt/Ardalis.Result.AspNetCore.svg)](https://www.nuget.org/packages/Ardalis.Result.AspNetCore)


[![.NET Core](https://github.com/ardalis/Result/workflows/.NET%20Core/badge.svg)](https://github.com/ardalis/Result/actions?query=workflow%3A%22.NET+Core%22)

# Result
A result abstraction that can be mapped to HTTP response codes if needed.

## Getting Started

If you're building an ASP.NET Core Web API you can simply install the [Ardalis.Result.AspNetCore](https://www.nuget.org/packages/Ardalis.Result.AspNetCore/) package to get started. Then, apply the `[TranslateResultToActionResult]` attribute to any actions or controllers that you want to automatically translate from Result types to ActionResult types.

