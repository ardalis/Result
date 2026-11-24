using Xunit;
using static Ardalis.Result.Result;
using static Ardalis.Result.Shouldly.UnitTests.Utils.Constants;

namespace Ardalis.Result.Shouldly.UnitTests.FailureResults;

public class UnavailableResult
{
    [Fact]
    public void ShouldBeFailure()
    {
        Unavailable().ShouldBeFailure();
    }

    [Fact]
    public void WithErrorMessages_ShouldBeFailureWithErrorMessages()
    {
        Unavailable(ErrorMessage).ShouldBeFailure(ErrorMessage);
    }

    [Fact]
    public void ShouldBeUnavailable()
    {
        Unavailable().ShouldBeUnavailable();
    }

    [Fact]
    public void WithErrorMessages_ShouldBeUnavailableWithErrorMessages()
    {
        Unavailable(ErrorMessage).ShouldBeUnavailable(ErrorMessage);
    }

    [Fact]
    public void WithErrorMessages_ShouldBeUnavailable()
    {
        Unavailable(ErrorMessage).ShouldBeUnavailable();
    }
}