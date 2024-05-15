using System;
using System.Linq;
using System.Text;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

#if NET7_0_OR_GREATER
namespace Ardalis.Result.AspNetCore;

public static partial class ResultExtensions
{
    /// <summary>
    /// Convert a <see cref="Result{T}"/> to an instance of <see cref="Microsoft.AspNetCore.Http.IResult"/>
    /// </summary>
    /// <typeparam name="T">The value being returned</typeparam>
    /// <param name="result">The Ardalis.Result to convert to an Microsoft.AspNetCore.Http.IResult</param>
    /// <returns></returns>
    public static Microsoft.AspNetCore.Http.IResult ToMinimalApiResult<T>(this Result<T> result)
    {
        return ToMinimalApiResult((IResult)result);
    }

    /// <summary>
    /// Convert a <see cref="Result"/> to an instance of <see cref="Microsoft.AspNetCore.Http.IResult"/>
    /// </summary>
    /// <param name="result">The Ardalis.Result to convert to an Microsoft.AspNetCore.Http.IResult</param>
    /// <returns></returns>
    public static Microsoft.AspNetCore.Http.IResult ToMinimalApiResult(this Result result)
    {
        return ToMinimalApiResult((IResult)result);
    }

    internal static Microsoft.AspNetCore.Http.IResult ToMinimalApiResult(this IResult result) =>
        result.Status switch
        {
            ResultStatus.Ok => typeof(Result).IsInstanceOfType(result)
                ? Results.Ok()
                : Results.Ok(result.GetValue()),
            ResultStatus.NotFound => NotFoundEntity(result),
            ResultStatus.Unauthorized => Results.Unauthorized(),
            ResultStatus.Forbidden => Results.Forbid(),
            ResultStatus.Invalid => Results.BadRequest(result.ValidationErrors),
            ResultStatus.Error => UnprocessableEntity(result),
            ResultStatus.Conflict => ConflictEntity(result),
            ResultStatus.Unavailable => UnavailableEntity(result),
            ResultStatus.CriticalError => CriticalEntity(result),
            _ => throw new NotSupportedException($"Result {result.Status} conversion is not supported."),
        };

    private static Microsoft.AspNetCore.Http.IResult UnprocessableEntity(IResult result)
    {
        var details = new StringBuilder("Next error(s) occurred:");

        foreach (var error in result.Errors) details.Append("* ").Append(error).AppendLine();

        return Results.UnprocessableEntity(new ProblemDetails
        {
            Title = "Something went wrong.",
            Detail = details.ToString()
        });
    }

    private static Microsoft.AspNetCore.Http.IResult NotFoundEntity(IResult result)
    {
        var details = new StringBuilder("Next error(s) occurred:");

        if (result.Errors.Any())
        {
            foreach (var error in result.Errors) details.Append("* ").Append(error).AppendLine();

            return Results.NotFound(new ProblemDetails
            {
                Title = "Resource not found.",
                Detail = details.ToString()
            });
        }
        else
        {
            return Results.NotFound();
        }
    }

    private static Microsoft.AspNetCore.Http.IResult ConflictEntity(IResult result)
    {
        var details = new StringBuilder("Next error(s) occurred:");

        if (result.Errors.Any())
        {
            foreach (var error in result.Errors) details.Append("* ").Append(error).AppendLine();

            return Results.Conflict(new ProblemDetails
            {
                Title = "There was a conflict.",
                Detail = details.ToString()
            });
        }
        else
        {
            return Results.Conflict();
        }
    }

    private static Microsoft.AspNetCore.Http.IResult CriticalEntity(IResult result)
    {
        var details = new StringBuilder("Next error(s) occurred:");

        if (result.Errors.Any())
        {
            foreach (var error in result.Errors) details.Append("* ").Append(error).AppendLine();

            return Results.Problem(new ProblemDetails()
            {
                Title = "Something went wrong.",
                Detail = details.ToString(),
                Status = StatusCodes.Status500InternalServerError
            });
        }
        else
        {
            return Results.StatusCode(StatusCodes.Status500InternalServerError);
        }
    }

    private static Microsoft.AspNetCore.Http.IResult UnavailableEntity(IResult result)
    {
        var details = new StringBuilder("Next error(s) occurred:");

        if (result.Errors.Any())
        {
            foreach (var error in result.Errors) details.Append("* ").Append(error).AppendLine();
            
            return Results.Problem(new ProblemDetails
            {
                Title = "Service unavailable.",
                Detail = details.ToString(),
                Status = StatusCodes.Status503ServiceUnavailable
            });
        }
        else
        {
            return Results.StatusCode(StatusCodes.Status503ServiceUnavailable);
        }
    }
}
#endif
