using Xunit;

namespace Ardalis.Result.FluentAssertions.UnitTests.FailureResults;

public class InvalidResult
{
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
    public void InvalidResult_ShouldBeInvalid()
    {
        Result.Invalid().ShouldBeInvalid();
    }

    [Fact]
    public void InvalidResultWithValidationError_ShouldBeInvalidWithValidationError()
    {
        var validationError = new ValidationError("IDENTIFIER", "ERROR_MESSAGE");
        Result.Invalid(validationError).ShouldBeInvalid(validationError);
    }
}