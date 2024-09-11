#if NET6_0_OR_GREATER

using Ardalis.Result.AspNetCore;
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
}
#endif