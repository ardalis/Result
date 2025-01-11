using Xunit;
using static Ardalis.Result.Result;

namespace Ardalis.Result.FluentAssertions.UnitTests.FailureResults;

public class InvalidResult
{
    private static readonly ValidationError ValidationError = new("IDENTIFIER", "ERROR_MESSAGE", "ERROR_CODE", ValidationSeverity.Error);
    
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
        Invalid(ValidationError).ShouldBeInvalid(ValidationError);
    }
    
    [Fact]
    public void WithValidationError_ShouldBeInvalid()
    {
        Invalid(ValidationError).ShouldBeInvalid();
    }
    
    [Fact]
    public void ShouldHaveValidationErrorWithCode()
    {
        Invalid(ValidationError).ShouldHaveValidationErrorWithCode("ERROR_CODE");
    }
}