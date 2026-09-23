using Xunit;
using static Ardalis.Result.Result;
using static Ardalis.Result.Shouldly.UnitTests.Utils.Constants;

namespace Ardalis.Result.Shouldly.UnitTests.FailureResults;

public class ConflictResult
{
    [Fact]
    public void ShouldBeFailure()
    {
        Conflict().ShouldBeFailure();
    }

    [Fact]
    public void WithErrorMessages_ShouldBeFailureWithErrorMessages()
    {
        Conflict(ErrorMessage).ShouldBeFailure(ErrorMessage);
    }

    [Fact]
    public void ShouldBeConflict()
    {
        Conflict().ShouldBeConflict();
    }

    [Fact]
    public void WithErrorMessages_ShouldBeConflictWithErrorMessages()
    {
        Conflict(ErrorMessage).ShouldBeConflict(ErrorMessage);
    }

    [Fact]
    public void WithMultipleErrorMessages_ShouldBeConflictWithErrorMessages()
    {
        Conflict("Error 1", "Error 2")
            .ShouldBeConflict("Error 1", "Error 2");
    }
}