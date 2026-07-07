#if NET6_0_OR_GREATER
#nullable enable
using System;

using Microsoft.AspNetCore.Mvc;

namespace Ardalis.Result.AspNetCore;

/// <summary>
/// Context passed to <see cref="ResultProblemDetailsOptions.Customize"/> when the Minimal API
/// <c>ToMinimalApiResult</c> extensions build a <see cref="ProblemDetails"/> for an error-class result.
/// </summary>
public sealed class ResultProblemDetailsContext
{
    /// <summary>The Ardalis.Result status that produced this response.</summary>
    public required ResultStatus ResultStatus { get; init; }

    /// <summary>The HTTP status code that will be returned.</summary>
    public required int StatusCode { get; init; }

    /// <summary>The result being converted.</summary>
    public required IResult Result { get; init; }

    /// <summary>
    /// The <see cref="ProblemDetails"/> about to be returned. Mutate it in place (Title, Detail,
    /// Extensions, ...) to customize the response.
    /// </summary>
    public required ProblemDetails ProblemDetails { get; init; }
}

/// <summary>
/// Global, configure-once customization for the <see cref="ProblemDetails"/> produced by the Minimal API
/// <c>ToMinimalApiResult</c> extensions. Mirrors the spirit of ASP.NET Core's
/// <c>ProblemDetailsOptions.CustomizeProblemDetails</c>, but applies to the Ardalis.Result conversion which
/// runs without access to an <c>HttpContext</c>.
/// </summary>
public static class ResultProblemDetailsOptions
{
    /// <summary>
    /// Optional hook to customize the <see cref="ProblemDetails"/> produced for error-class results. Null by
    /// default, leaving the library's formatting unchanged. Set once at application startup, e.g. to remove the
    /// default "Next error(s) occurred:" Detail prefix or to localize the Title.
    /// </summary>
    public static Action<ResultProblemDetailsContext>? Customize { get; set; }
}
#endif