using Xunit;
using static Ardalis.Result.Result;
using static Ardalis.Result.FluentAssertions.UnitTests.Utils.Constants;

namespace Ardalis.Result.FluentAssertions.UnitTests.FailureResults;

public class UnauthorizedResult
{
    [Fact]
    public void ShouldBeFailure()
    {
        Unauthorized().ShouldBeFailure();
    }

    [Fact]
    public void WithErrorMessages_ShouldBeFailureWithErrorMessages()
    {
        Unauthorized(ErrorMessage).ShouldBeFailure(ErrorMessage);
    }

    [Fact]
    public void ShouldBeUnauthorized()
    {
        Unauthorized().ShouldBeUnauthorized();
    }

    [Fact]
    public void WithErrorMessages_ShouldBeUnauthorizedWithErrorMessages()
    {
        Unauthorized(ErrorMessage).ShouldBeUnauthorized(ErrorMessage);
    }
}