using FluentAssertions;
using FluentAssertions.Primitives;
using Xunit;
using Xunit.Sdk;

namespace Ardalis.Result.FluentAssertions.UnitTests;

public class FluentAssertionsResultExtensionsTests
{
    private const string ErrorMessage = "Error message"; 
    
    //ShouldBeFailure

    [Fact]
    public void ConflictResult_ShouldBeFailure()
    {
        Result.Conflict().ShouldBeFailure();
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

    [Fact]
    public void ConflictResultWithErrorMessages_ShouldBeFailureWithErrorMessages()
    {
        Result.Conflict(ErrorMessage).ShouldBeFailure(ErrorMessage);
    }


    [Fact]
    public void CriticalErrorResult_ShouldBeFailure()
    {
        Result.CriticalError().ShouldBeFailure();
    }

    [Fact]
    public void ErrorResult_ShouldBeFailure()
    {
        Result.Error().ShouldBeFailure();
    }

    [Fact]
    public void ForbiddenResult_ShouldBeFailure()
    {
        Result.Forbidden().ShouldBeFailure();
    }

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
    public void CriticalErrorResultWithMessage_ShouldBeFailureWithMessage()
    {
        Result.CriticalError(ErrorMessage).ShouldBeFailure(ErrorMessage);
    }
    
    [Fact]
    public void ErrorResultWithMessage_ShouldBeFailureWithMessage()
    {
        Result.Error(ErrorMessage).ShouldBeFailure(ErrorMessage);
    }
    
    [Fact]
    public void ForbiddenResultWithMessage_ShouldBeFailureWithMessage()
    {
        Result.Forbidden(ErrorMessage).ShouldBeFailure(ErrorMessage);
    }

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