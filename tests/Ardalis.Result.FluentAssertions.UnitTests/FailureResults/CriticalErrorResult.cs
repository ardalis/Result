using Xunit;
using static Ardalis.Result.Result;
using static Ardalis.Result.FluentAssertions.UnitTests.Utils.Constants;

namespace Ardalis.Result.FluentAssertions.UnitTests.FailureResults;

public class CriticalErrorResult
{
    [Fact]
    public void ShouldBeFailure()
    {
        CriticalError().ShouldBeFailure();
    }

    [Fact]
    public void WithErrorMessages_ShouldBeFailureWithErrorMessages()
    {
        CriticalError(ErrorMessage).ShouldBeFailure(ErrorMessage);
    }

    [Fact]
    public void ShouldBeCriticalError()
    {
        CriticalError().ShouldBeCriticalError();
    }

    [Fact]
    public void ShouldBeCriticalErrorWithErrorMessages()
    {
        CriticalError(ErrorMessage).ShouldBeCriticalError(ErrorMessage);
    }
}