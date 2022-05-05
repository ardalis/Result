namespace Ardalis.Result.Net6;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

using System.Text;

public static class IResultExtensions
{
	/// <summary>
	/// Convert an Ardalis.Result to a Microsoft.AspNetCore.Http.IResult
	/// </summary>
	/// <typeparam name="T">The value being returned</typeparam>
	/// <param name="result">The Result to convert to an IResult</param>
	/// <returns></returns>
	public static IResult ToIResult<T>(this Result<T> result)
	=> result.Status switch
	{
		ResultStatus.Ok => Results.Ok(result.GetValue()),
		ResultStatus.Error => UnprocessableEntity(result.Errors),
		ResultStatus.Forbidden => Results.Forbid(),
		ResultStatus.Unauthorized => Results.Unauthorized(),
		ResultStatus.Invalid => BadRequest(result.ValidationErrors),
		ResultStatus.NotFound => Results.NotFound(),
		_ => throw new NotSupportedException($"Result {result.Status} conversion is not supported.")
	};

	private static IResult BadRequest(List<ValidationError> validationErrors)
	=> Results.ValidationProblem(validationErrors.ToErrorsDictionary());

	private static IResult UnprocessableEntity(IEnumerable<string> errors)
	{
		var details = new StringBuilder("Next error(s) occured:");

		foreach (var error in errors) details.Append("* ").Append(error).AppendLine();

		return Results.UnprocessableEntity(new ProblemDetails
		{
			Title = "Something went wrong.",
			Detail = details.ToString()
		});
	}

	public static Dictionary<string, string[]> ToErrorsDictionary(this IEnumerable<ValidationError> validationErrors)
	=> validationErrors
		.Where(e => e != null)
		.GroupBy(e => e.Identifier, e => e.ErrorMessage)
		.ToDictionary(failureGroup => failureGroup.Key, failureGroup => failureGroup.ToArray());
}