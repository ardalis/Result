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
            _ => throw new NotSupportedException($"Result {result.Status} conversion is not supported."),
        };

    private static Microsoft.AspNetCore.Http.IResult UnprocessableEntity(IResult result)
    {
        var details = new StringBuilder("Next error(s) occured:");

        foreach (var error in result.Errors) details.Append("* ").Append(error).AppendLine();

        return Results.UnprocessableEntity(new ProblemDetails
        {
            Title = "Something went wrong.",
            Detail = details.ToString()
        });
    }

    private static Microsoft.AspNetCore.Http.IResult NotFoundEntity(IResult result)
    {
        var details = new StringBuilder("Next error(s) occured:");

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
}
#endif
