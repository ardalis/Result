using Xunit;

namespace Ardalis.Result.FluentAssertions.UnitTests.FailureResults;

public class ForbiddenResult
{
    private const string ErrorMessage = "Error message"; 

    [Fact]
    public void ForbiddenResult_ShouldBeFailure()
    {
        Result.Forbidden().ShouldBeFailure();
    }

    [Fact]
    public void ForbiddenResultWithMessage_ShouldBeFailureWithMessage()
    {
        Result.Forbidden(ErrorMessage).ShouldBeFailure(ErrorMessage);
    }

    [Fact]
    public void ForbiddenResult_ShouldBeForbidden()
    {
        Result.Forbidden().ShouldBeForbidden();
    }

    [Fact]
    public void ForbiddenResultWithMessage_ShouldBeForbiddenWithMessage()
    {
        Result.Forbidden(ErrorMessage).ShouldBeForbidden(ErrorMessage);
    }
}