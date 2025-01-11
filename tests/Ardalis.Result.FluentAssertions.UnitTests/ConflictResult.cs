using Moq;
using Xunit;
using static Ardalis.Result.Result;

namespace Ardalis.Result.FluentAssertions.UnitTests;

public class ConflictResult
{
    private const string ErrorMessage = "Error message"; 
    
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
}