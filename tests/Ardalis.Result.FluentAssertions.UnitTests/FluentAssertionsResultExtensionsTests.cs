using Moq;
using Xunit;
using static Ardalis.Result.Result;

namespace Ardalis.Result.FluentAssertions.UnitTests;

public class FluentAssertionsResultExtensionsTests
{
    private const string ErrorMessage = "Error message"; 
    
    //CriticalError

    [Fact]
    public void CriticalErrorResult_ShouldBeFailure()
    {
        CriticalError().ShouldBeFailure();
    }

    [Fact]
    public void CriticalErrorResultWithErrorMessages_ShouldBeFailureWithErrorMessages()
    {
        CriticalError(ErrorMessage).ShouldBeFailure(ErrorMessage);
    }

    [Fact]
    public void CriticalErrorResult_ShouldBeCriticalError()
    {
        CriticalError().ShouldBeCriticalError();
    }

    [Fact]
    public void CriticalErrorResultWithErrorMessages_ShouldBeCriticalErrorWithErrorMessages()
    {
        CriticalError(ErrorMessage).ShouldBeCriticalError(ErrorMessage);
    }


    //Error

    [Fact]
    public void ErrorResult_ShouldBeFailure()
    {
        Error().ShouldBeFailure();
    }

    [Fact]
    public void ErrorResultWithErrorMessages_ShouldBeFailureWithErrorMessages()
    {
        Error(ErrorMessage).ShouldBeFailure(ErrorMessage);
    }

    [Fact]
    public void ErrorResult_ShouldBeError()
    {
        Error().ShouldBeError();
    }

    [Fact]
    public void ErrorResultWithErrorMessage_ShouldBeErrorWithErrorMessage()
    {
        Error(ErrorMessage).ShouldBeError(ErrorMessage);
    }

    [Fact]
    public void ErrorResultWithErrorList_ShouldBeErrorWithErrorList()
    {
        var errorList = new ErrorList([ErrorMessage], "CorrelationId");
        
        Error(errorList).ShouldBeError(errorList);
    }

    [Fact]
    public void ErrorResultWithErrorList_ShouldBeErrorWithErrorMessagesAndCorrelationId()
    {
        var errorList = new ErrorList([ErrorMessage], "CorrelationId");

        Error(errorList).ShouldBeError([ErrorMessage], "CorrelationId");
    }

    //Forbidden

    [Fact]
    public void ForbiddenResult_ShouldBeFailure()
    {
        Forbidden().ShouldBeFailure();
    }

    [Fact]
    public void ForbiddenResultWithMessage_ShouldBeFailureWithMessage()
    {
        Forbidden(ErrorMessage).ShouldBeFailure(ErrorMessage);
    }

    [Fact]
    public void ForbiddenResult_ShouldBeForbidden()
    {
        Forbidden().ShouldBeForbidden();
    }

    [Fact]
    public void ForbiddenResultWithMessage_ShouldBeForbiddenWithMessage()
    {
        Forbidden(ErrorMessage).ShouldBeForbidden(ErrorMessage);
    }

    //Invalid
    

    [Fact]
    public void InvalidResult_ShouldBeFailure()
    {
        Invalid().ShouldBeFailure();
    }

    [Fact]
    public void InvalidResultWithValidationError_ShouldBeFailure()
    {
        Invalid(new ValidationError("IDENTIFIER", "ERROR_MESSAGE")).ShouldBeFailure();
    }

    [Fact]
    public void InvalidResult_ShouldBeInvalid()
    {
        Invalid().ShouldBeInvalid();
    }

    [Fact]
    public void InvalidResultWithValidationError_ShouldBeInvalidWithValidationError()
    {
        var validationError = new ValidationError("IDENTIFIER", "ERROR_MESSAGE");
        Invalid(validationError).ShouldBeInvalid(validationError);
    }
    
    //NotFound

    [Fact]
    public void NotFoundResult_ShouldBeFailure()
    {
        NotFound().ShouldBeFailure();
    }

    [Fact]
    public void NotFoundResultWithMessage_ShouldBeFailureWithMessage()
    {
        NotFound(ErrorMessage).ShouldBeFailure(ErrorMessage);
    }

    [Fact]
    public void NotFoundResult_ShouldBeNotFound()
    {
        NotFound().ShouldBeNotFound();
    }

    [Fact]
    public void NotFoundResultWithErrorMessages_ShouldBeNotFoundWithErrorMessages()
    {
        NotFound(ErrorMessage).ShouldBeNotFound(ErrorMessage);
    }

    //Unauthorized

    [Fact]
    public void UnauthorizedResult_ShouldBeFailure()
    {
        Unauthorized().ShouldBeFailure();
    }

    [Fact]
    public void UnauthorizedResultWithErrorMessages_ShouldBeFailureWithErrorMessages()
    {
        Unauthorized(ErrorMessage).ShouldBeFailure(ErrorMessage);
    }

    [Fact]
    public void UnauthorizedResult_ShouldBeUnauthorized()
    {
        Unauthorized().ShouldBeUnauthorized();
    }

    [Fact]
    public void UnauthorizedResultWithErrorMessages_ShouldBeUnauthorizedWithErrorMessages()
    {
        Unauthorized(ErrorMessage).ShouldBeUnauthorized(ErrorMessage);
    }

    //Unavailable

    [Fact]
    public void UnavailableResult_ShouldBeFailure()
    {
        Unavailable().ShouldBeFailure();
    }

    [Fact]
    public void UnavailableResultWithErrorMessages_ShouldBeFailureWithErrorMessages()
    {
        Unavailable(ErrorMessage).ShouldBeFailure(ErrorMessage);
    }

    [Fact]
    public void UnavailableResult_ShouldBeUnavailable()
    {
        Unavailable().ShouldBeUnavailable();
    }

    [Fact]
    public void UnavailableResultWithErrorMessages_ShouldBeUnavailableWithErrorMessages()
    {
        Unavailable(ErrorMessage).ShouldBeUnavailable(ErrorMessage);
    }
}