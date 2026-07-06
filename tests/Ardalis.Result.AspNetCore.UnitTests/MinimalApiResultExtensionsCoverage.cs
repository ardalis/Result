#if NET6_0_OR_GREATER

using System.Reflection;

using Ardalis.Result.AspNetCore;
using Microsoft.AspNetCore.Mvc;
using Xunit;
using Xunit.Abstractions;

namespace Ardalis.Result.AspNetCore.UnitTests;

public class MinimalApiResultExtensionsCoverage : BaseResultConventionTest
{
    private readonly ITestOutputHelper _testOutputHelper;

    public MinimalApiResultExtensionsCoverage(ITestOutputHelper testOutputHelper)
    {
        _testOutputHelper = testOutputHelper;
    }

    private class TestResult(ResultStatus status) : Result(status);
    
    [Fact]
    public void ToMinimalApiResultHandlesAllResultStatusValues()
    {
        foreach (ResultStatus resultStatus in Enum.GetValues(typeof(ResultStatus)))
        {
#if NET7_0
            // Results.Created does not accept empty string URI in net7
            if (resultStatus == ResultStatus.Created)
            {
                continue;
            }
#endif
            Result result = new TestResult(resultStatus);
            try
            {
                Microsoft.AspNetCore.Http.IResult minimalApiResult = result.ToMinimalApiResult();
            }
            catch (NotSupportedException e)
            {
                Assert.Fail(
                    $"Unhandled ResultStatus {resultStatus} in MinimalApiResultExtensions.ToMinimalApiResult: {e}");
            }
        }
    }

    [Fact]
    public void ErrorResultPopulatesProblemDetailsStatus()
    {
        var problemDetails = ExtractProblemDetails(Result<int>.Error("boom").ToMinimalApiResult());
        Assert.Equal(422, problemDetails.Status);
    }

    [Fact]
    public void NotFoundResultWithErrorsPopulatesProblemDetailsStatus()
    {
        var problemDetails = ExtractProblemDetails(Result<int>.NotFound("missing").ToMinimalApiResult());
        Assert.Equal(404, problemDetails.Status);
    }

    [Fact]
    public void ConflictResultWithErrorsPopulatesProblemDetailsStatus()
    {
        var problemDetails = ExtractProblemDetails(Result<int>.Conflict("conflict").ToMinimalApiResult());
        Assert.Equal(409, problemDetails.Status);
    }

    [Fact]
    public void CriticalErrorResultStillPopulatesProblemDetailsStatus()
    {
        var problemDetails = ExtractProblemDetails(Result<int>.CriticalError("boom").ToMinimalApiResult());
        Assert.Equal(500, problemDetails.Status);
    }

    // The concrete result type returned by Results.UnprocessableEntity/NotFound/Conflict/Problem differs across
    // target frameworks (and is internal on net6), so read the ProblemDetails reflectively by well-known property name.
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