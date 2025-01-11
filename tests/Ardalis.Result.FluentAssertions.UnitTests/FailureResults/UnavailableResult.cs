using Xunit;
using static Ardalis.Result.Result;
using static Ardalis.Result.FluentAssertions.UnitTests.Utils.Constants;

namespace Ardalis.Result.FluentAssertions.UnitTests.FailureResults;

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
}