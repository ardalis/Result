#if NET6_0_OR_GREATER

using System;
using System.Reflection;

using Ardalis.Result.AspNetCore;
using Microsoft.AspNetCore.Mvc;
using Xunit;

namespace Ardalis.Result.AspNetCore.UnitTests;

public class MinimalApiResultProblemDetailsCustomizationTests : IDisposable
{
    // Customize is global static; reset after every test so cases stay isolated.
    public void Dispose() => ResultProblemDetailsOptions.Customize = null;

    [Fact]
    public void WithoutCustomization_DefaultTitleAndDetailAreUnchanged()
    {
        ResultProblemDetailsOptions.Customize = null;

        var problemDetails = ExtractProblemDetails(Result<int>.Error("boom").ToMinimalApiResult());

        Assert.Equal("Something went wrong.", problemDetails.Title);
        Assert.StartsWith("Next error(s) occurred:", problemDetails.Detail);
    }

    [Fact]
    public void Customize_CanRewriteTitleAndDetail()
    {
        ResultProblemDetailsOptions.Customize = context =>
        {
            context.ProblemDetails.Title = "Custom title";
            context.ProblemDetails.Detail = string.Join("; ", context.Result.Errors);
        };

        var problemDetails = ExtractProblemDetails(Result<int>.Error("boom").ToMinimalApiResult());

        Assert.Equal("Custom title", problemDetails.Title);
        Assert.Equal("boom", problemDetails.Detail);
    }

    [Fact]
    public void Customize_ReceivesResultStatusAndStatusCode()
    {
        ResultProblemDetailsContext captured = null;
        ResultProblemDetailsOptions.Customize = context => captured = context;

        Result<int>.Conflict("nope").ToMinimalApiResult();

        Assert.NotNull(captured);
        Assert.Equal(ResultStatus.Conflict, captured.ResultStatus);
        Assert.Equal(409, captured.StatusCode);
    }

    // The concrete result type returned by Results.UnprocessableEntity/Conflict/Problem differs across target
    // frameworks (and is internal on net6), so read the ProblemDetails reflectively by well-known property name.
    private static ProblemDetails ExtractProblemDetails(Microsoft.AspNetCore.Http.IResult result)
    {
        const BindingFlags flags = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance;
        var type = result.GetType();
        foreach (var propertyName in new[] { "ProblemDetails", "Value" })
        {
            if (type.GetProperty(propertyName, flags)?.GetValue(result) is ProblemDetails problemDetails)
            {
                return problemDetails;
            }
        }
        throw new InvalidOperationException($"Could not extract ProblemDetails from {type.FullName}");
    }
}
#endif