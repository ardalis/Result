using Xunit;
using static Ardalis.Result.Result;
using static Ardalis.Result.Shouldly.UnitTests.Utils.Constants;

namespace Ardalis.Result.Shouldly.UnitTests.FailureResults;

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
    public void WithErrorMessages_ShouldBeUnauthorized()
    {
        Unauthorized(ErrorMessage).ShouldBeUnauthorized();
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