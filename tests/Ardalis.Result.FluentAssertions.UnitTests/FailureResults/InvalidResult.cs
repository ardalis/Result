using Xunit;
using static Ardalis.Result.Result;

namespace Ardalis.Result.FluentAssertions.UnitTests.FailureResults;

public class InvalidResult
{
    [Fact]
    public void ShouldBeFailure()
    {
        Invalid().ShouldBeFailure();
    }

    [Fact]
    public void WithValidationError_ShouldBeFailure()
    {
        Invalid().ShouldBeFailure();
    }

    [Fact]
    public void ShouldBeInvalid()
    {
        Invalid().ShouldBeInvalid();
    }

    [Fact]
    public void WithValidationError_ShouldBeInvalidWithValidationError()
    {
        var validationError = new ValidationError("IDENTIFIER", "ERROR_MESSAGE", "ERROR_CODE", ValidationSeverity.Error);
        
        Invalid(validationError)
            .ShouldBeInvalid(validationError);
    }
    
    [Fact]
    public void WithValidationError_ShouldBeInvalid()
    {
        var validationError = new ValidationError("IDENTIFIER", "ERROR_MESSAGE", "ERROR_CODE", ValidationSeverity.Error);
        
        Invalid(validationError).ShouldBeInvalid();
    }
    
    [Fact]
    public void ShouldHaveValidationErrorWithCode()
    {
        var validationError = new ValidationError("IDENTIFIER", "ERROR_MESSAGE", "ERROR_CODE", ValidationSeverity.Error);
        
        Invalid(validationError).ShouldHaveValidationErrorWithCode("ERROR_CODE");
    }
    
    [Fact]
    public void ShouldHaveValidationErrorWithIdentifier()
    {
        var validationError = new ValidationError("IDENTIFIER", "ERROR_MESSAGE", "ERROR_CODE", ValidationSeverity.Error);
        
        Invalid(validationError).ShouldHaveValidationErrorWithIdentifier("IDENTIFIER");
    }
    
    [Fact]
    public void ShouldHaveValidationErrorWithErrorMessage()
    {
        var validationError = new ValidationError("IDENTIFIER", "ERROR_MESSAGE", "ERROR_CODE", ValidationSeverity.Error);
        
        Invalid(validationError).ShouldHaveValidationErrorWithMessage("ERROR_MESSAGE");
    }
    
    [Fact]
    public void ShouldHaveValidationErrorWithSeverity()
    {
        var validationError = new ValidationError("IDENTIFIER", "ERROR_MESSAGE", "ERROR_CODE", ValidationSeverity.Error);
        
        Invalid(validationError).ShouldHaveValidationErrorWithSeverity(ValidationSeverity.Error);
    }
}