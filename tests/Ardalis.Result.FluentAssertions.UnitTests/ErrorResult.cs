using Xunit;

namespace Ardalis.Result.FluentAssertions.UnitTests;

public class ErrorResult
{
    private const string ErrorMessage = "Error message"; 
    
    [Fact]
    public void ErrorResult_ShouldBeFailure()
    {
        Result.Error().ShouldBeFailure();
    }

    [Fact]
    public void ErrorResultWithErrorMessages_ShouldBeFailureWithErrorMessages()
    {
        Result.Error(ErrorMessage).ShouldBeFailure(ErrorMessage);
    }

    [Fact]
    public void ErrorResult_ShouldBeError()
    {
        Result.Error().ShouldBeError();
    }

    [Fact]
    public void ErrorResultWithErrorMessage_ShouldBeErrorWithErrorMessage()
    {
        Result.Error(ErrorMessage).ShouldBeError(ErrorMessage);
    }

    [Fact]
    public void ErrorResultWithErrorList_ShouldBeErrorWithErrorList()
    {
        var errorList = new ErrorList([ErrorMessage], "CorrelationId");
        
        Result.Error(errorList).ShouldBeError(errorList);
    }

    [Fact]
    public void ErrorResultWithErrorList_ShouldBeErrorWithErrorMessagesAndCorrelationId()
    {
        var errorList = new ErrorList([ErrorMessage], "CorrelationId");

        Result.Error(errorList).ShouldBeError([ErrorMessage], "CorrelationId");
    }
}