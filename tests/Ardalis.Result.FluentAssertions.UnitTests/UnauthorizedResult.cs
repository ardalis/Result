using Xunit;

namespace Ardalis.Result.FluentAssertions.UnitTests;

public class UnauthorizedResult
{
    private const string ErrorMessage = "Error message"; 
    
    [Fact]
    public void UnauthorizedResult_ShouldBeFailure()
    {
        Result.Unauthorized().ShouldBeFailure();
    }

    [Fact]
    public void UnauthorizedResultWithErrorMessages_ShouldBeFailureWithErrorMessages()
    {
        Result.Unauthorized(ErrorMessage).ShouldBeFailure(ErrorMessage);
    }

    [Fact]
    public void UnauthorizedResult_ShouldBeUnauthorized()
    {
        Result.Unauthorized().ShouldBeUnauthorized();
    }

    [Fact]
    public void UnauthorizedResultWithErrorMessages_ShouldBeUnauthorizedWithErrorMessages()
    {
        Result.Unauthorized(ErrorMessage).ShouldBeUnauthorized(ErrorMessage);
    }
}