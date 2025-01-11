using Xunit;

namespace Ardalis.Result.FluentAssertions.UnitTests.FailureResults;

public class CriticalErrorResult
{
    private const string ErrorMessage = "Error message"; 
    
    //CriticalError

    [Fact]
    public void CriticalErrorResult_ShouldBeFailure()
    {
        Result.CriticalError().ShouldBeFailure();
    }

    [Fact]
    public void CriticalErrorResultWithErrorMessages_ShouldBeFailureWithErrorMessages()
    {
        Result.CriticalError(ErrorMessage).ShouldBeFailure(ErrorMessage);
    }

    [Fact]
    public void CriticalErrorResult_ShouldBeCriticalError()
    {
        Result.CriticalError().ShouldBeCriticalError();
    }

    [Fact]
    public void CriticalErrorResultWithErrorMessages_ShouldBeCriticalErrorWithErrorMessages()
    {
        Result.CriticalError(ErrorMessage).ShouldBeCriticalError(ErrorMessage);
    }
}