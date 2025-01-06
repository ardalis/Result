using Xunit;

namespace Ardalis.Result.FluentAssertions.UnitTests;

public class FluentAssertionsResultExtensionsTests
{
    private const string ErrorMessage = "Error message"; 
    
    //Conflict

    [Fact]
    public void ConflictResult_ShouldBeFailure()
    {
        Result.Conflict().ShouldBeFailure();
    }

    [Fact]
    public void ConflictResultWithErrorMessages_ShouldBeFailureWithErrorMessages()
    {
        Result.Conflict(ErrorMessage).ShouldBeFailure(ErrorMessage);
    }

    [Fact]
    public void ConflictResult_ShouldBeConflict()
    {
        Result.Conflict().ShouldBeConflict();
    }

    [Fact]
    public void ConflictResultWithErrorMessages_ShouldBeConflictWithErrorMessages()
    {
        Result.Conflict(ErrorMessage).ShouldBeConflict(ErrorMessage);
    }



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


    //Error

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

    //Forbidden

    [Fact]
    public void ForbiddenResult_ShouldBeFailure()
    {
        Result.Forbidden().ShouldBeFailure();
    }

    [Fact]
    public void ForbiddenResultWithMessage_ShouldBeFailureWithMessage()
    {
        Result.Forbidden(ErrorMessage).ShouldBeFailure(ErrorMessage);
    }

    [Fact]
    public void ForbiddenResult_ShouldBeForbidden()
    {
        Result.Forbidden().ShouldBeForbidden();
    }

    [Fact]
    public void ForbiddenResultWithMessage_ShouldBeForbiddenWithMessage()
    {
        Result.Forbidden(ErrorMessage).ShouldBeForbidden(ErrorMessage);
    }

    //Invalid
    

    [Fact]
    public void InvalidResult_ShouldBeFailure()
    {
        Result.Invalid().ShouldBeFailure();
    }

    [Fact]
    public void InvalidResultWithValidationError_ShouldBeFailure()
    {
        Result.Invalid(new ValidationError("IDENTIFIER", "ERROR_MESSAGE")).ShouldBeFailure();
    }

    [Fact]
    public void InvalidResultr_ShouldBeInvalid()
    {
        Result.Invalid().ShoudBeInvalid();
    }





    [Fact]
    public void NotFoundResult_ShouldBeFailure()
    {
        Result.NotFound().ShouldBeFailure();
    }

    [Fact]
    public void UnauthorizedResult_ShouldBeFailure()
    {
        Result.Unauthorized().ShouldBeFailure();
    }

    [Fact]
    public void UnavailableResult_ShouldBeFailure()
    {
        Result.Unavailable().ShouldBeFailure();
    }

    //ShouldBEFailureWithMessage

    [Fact]
    public void NotFoundResultWithMessage_ShouldBeFailureWithMessage()
    {
        Result.NotFound(ErrorMessage).ShouldBeFailure(ErrorMessage);
    }
    
    [Fact]
    public void UnauthorizedResultWithMessage_ShouldBeFailureWithMessage()
    {
        Result.Unauthorized(ErrorMessage).ShouldBeFailure(ErrorMessage);
    }

    [Fact]
    public void UnavailableResultWithMessage_ShouldBeFailureWithMessage()
    {
        Result.Unavailable(ErrorMessage).ShouldBeFailure(ErrorMessage);
    }
}