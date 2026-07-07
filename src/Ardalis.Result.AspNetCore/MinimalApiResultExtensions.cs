using System;
using System.Linq;
using System.Text;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

#if NET6_0_OR_GREATER
namespace Ardalis.Result.AspNetCore;

public static partial class ResultExtensions
{
    /// <summary>
    /// Convert a <see cref="Result{T}"/> to an instance of <see cref="Microsoft.AspNetCore.Http.IResult"/>
    /// </summary>
    /// <typeparam name="T">The value being returned</typeparam>
    /// <param name="result">The Ardalis.Result to convert to an Microsoft.AspNetCore.Http.IResult</param>
    /// <returns></returns>
    public static Microsoft.AspNetCore.Http.IResult ToMinimalApiResult<T>(this Result<T> result) => ToMinimalApiResult((IResult)result);

    /// <summary>
    /// Convert a <see cref="Result"/> to an instance of <see cref="Microsoft.AspNetCore.Http.IResult"/>
    /// </summary>
    /// <param name="result">The Ardalis.Result to convert to an Microsoft.AspNetCore.Http.IResult</param>
    /// <returns></returns>
    public static Microsoft.AspNetCore.Http.IResult ToMinimalApiResult(this Result result) => ToMinimalApiResult((IResult)result);

    internal static Microsoft.AspNetCore.Http.IResult ToMinimalApiResult(this IResult result) =>
        result.Status switch
        {
            ResultStatus.Ok => result is Result ? Results.Ok() : Results.Ok(result.GetValue()),
            ResultStatus.Created => Results.Created("", result.GetValue()),
            ResultStatus.NoContent => Results.NoContent(),
            ResultStatus.NotFound => NotFoundEntity(result),
            ResultStatus.Unauthorized => UnAuthorized(result),
            ResultStatus.Forbidden => Forbidden(result),
            ResultStatus.Invalid => Results.BadRequest(result.ValidationErrors),
            ResultStatus.Error => UnprocessableEntity(result),
            ResultStatus.Conflict => ConflictEntity(result),
            ResultStatus.Unavailable => UnavailableEntity(result),
            ResultStatus.CriticalError => CriticalEntity(result),
            _ => throw new NotSupportedException($"Result {result.Status} conversion is not supported."),
        };

    // Invoke the optional global customization hook (if configured) on the ProblemDetails before returning it,
    // so consumers can adjust Title/Detail/Extensions without reimplementing the conversion. See ResultProblemDetailsOptions.
    private static ProblemDetails ApplyCustomization(IResult result, int statusCode, ProblemDetails problemDetails)
    {
        var customize = ResultProblemDetailsOptions.Customize;
        if (customize != null)
        {
            customize(new ResultProblemDetailsContext
            {
                ResultStatus = result.Status,
                StatusCode = statusCode,
                Result = result,
                ProblemDetails = problemDetails
            });
        }

        return problemDetails;
    }

    private static Microsoft.AspNetCore.Http.IResult UnprocessableEntity(IResult result)
    {
        var details = new StringBuilder("Next error(s) occurred:");

        foreach (var error in result.Errors) details.Append("* ").Append(error).AppendLine();

        return Results.UnprocessableEntity(ApplyCustomization(result, StatusCodes.Status422UnprocessableEntity, new ProblemDetails
        {
            Title = "Something went wrong.",
            Detail = details.ToString()
        }));
    }

    private static Microsoft.AspNetCore.Http.IResult NotFoundEntity(IResult result)
    {
        var details = new StringBuilder("Next error(s) occurred:");

        if (result.Errors.Any())
        {
            foreach (var error in result.Errors) details.Append("* ").Append(error).AppendLine();

            return Results.NotFound(ApplyCustomization(result, StatusCodes.Status404NotFound, new ProblemDetails
            {
                Title = "Resource not found.",
                Detail = details.ToString()
            }));
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

            return Results.Conflict(ApplyCustomization(result, StatusCodes.Status409Conflict, new ProblemDetails
            {
                Title = "There was a conflict.",
                Detail = details.ToString()
            }));
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

            return Results.Problem(ApplyCustomization(result, StatusCodes.Status500InternalServerError, new ProblemDetails()
            {
                Title = "Something went wrong.",
                Detail = details.ToString(),
                Status = StatusCodes.Status500InternalServerError
            }));
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
            
            return Results.Problem(ApplyCustomization(result, StatusCodes.Status503ServiceUnavailable, new ProblemDetails
            {
                Title = "Service unavailable.",
                Detail = details.ToString(),
                Status = StatusCodes.Status503ServiceUnavailable
            }));
        }
        else
        {
            return Results.StatusCode(StatusCodes.Status503ServiceUnavailable);
        }
    }

    private static Microsoft.AspNetCore.Http.IResult Forbidden(IResult result)
    {
        var details = new StringBuilder("Next error(s) occurred:");

        if (result.Errors.Any())
        {
            foreach (var error in result.Errors) details.Append("* ").Append(error).AppendLine();

            return Results.Problem(ApplyCustomization(result, StatusCodes.Status403Forbidden, new ProblemDetails
            {
                Title = "Forbidden.",
                Detail = details.ToString(),
                Status = StatusCodes.Status403Forbidden
            }));
        }
        else
        {
            return Results.Forbid();
        }
    }

    private static Microsoft.AspNetCore.Http.IResult UnAuthorized(IResult result)
    {
        var details = new StringBuilder("Next error(s) occurred:");

        if (result.Errors.Any())
        {
            foreach (var error in result.Errors) details.Append("* ").Append(error).AppendLine();

            return Results.Problem(ApplyCustomization(result, StatusCodes.Status401Unauthorized, new ProblemDetails
            {
                Title = "Unauthorized.",
                Detail = details.ToString(),
                Status = StatusCodes.Status401Unauthorized
            }));
        }
        else
        {
            return Results.Unauthorized();
        }
    }
}
#endif
