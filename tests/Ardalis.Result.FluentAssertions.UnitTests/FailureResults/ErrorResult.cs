using Xunit;
using static Ardalis.Result.Result;
using static Ardalis.Result.FluentAssertions.UnitTests.Utils.Constants;

namespace Ardalis.Result.FluentAssertions.UnitTests.FailureResults;

public class ErrorResult
{
    [Fact]
    public void ShouldBeFailure()
    {
        Error().ShouldBeFailure();
    }

    [Fact]
    public void WithErrorMessages_ShouldBeFailureWithErrorMessages()
    {
        Error(ErrorMessage).ShouldBeFailure(ErrorMessage);
    }

    [Fact]
    public void ShouldBeError()
    {
        Error().ShouldBeError();
    }

    [Fact]
    public void ShouldBeErrorWithErrorMessage()
    {
        Error(ErrorMessage).ShouldBeError(ErrorMessage);
    }

    [Fact]
    public void ShouldBeErrorWithErrorList()
    {
        var errorList = new ErrorList([ErrorMessage], "CorrelationId");
        
        Error(errorList).ShouldBeError(errorList);
    }

    [Fact]
    public void ShouldBeErrorWithErrorMessagesAndCorrelationId()
    {
        var errorList = new ErrorList([ErrorMessage], "CorrelationId");

        Error(errorList).ShouldBeError([ErrorMessage], "CorrelationId");
    }
}