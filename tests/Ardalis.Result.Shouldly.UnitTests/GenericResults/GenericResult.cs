using Xunit;
using static Ardalis.Result.Shouldly.UnitTests.Utils.Constants;

namespace Ardalis.Result.Shouldly.UnitTests.GenericResults;

// The assertions extend Result<T>, so they must work on typed results as well as on Result.
public class GenericResult
{
    [Fact]
    public void Success_ShouldBeSuccess()
    {
        Result<int>.Success(42).ShouldBeSuccess();
    }

    [Fact]
    public void SuccessWithMessage_ShouldBeSuccessWithMessage()
    {
        Result<int>.Success(42, "SUCCESS_MESSAGE").ShouldBeSuccessWithMessage("SUCCESS_MESSAGE");
    }

    [Fact]
    public void NotFound_ShouldBeNotFoundWithErrorMessages()
    {
        Result<int>.NotFound(ErrorMessage).ShouldBeNotFound(ErrorMessage);
    }

    [Fact]
    public void Error_ShouldBeErrorWithErrorList()
    {
        var errorList = new ErrorList(new[] { ErrorMessage }, "CORRELATION_ID");

        Result<int>.Error(errorList).ShouldBeError(errorList);
    }

    [Fact]
    public void Invalid_ShouldHaveValidationErrorWithIdentifier()
    {
        Result<int>.Invalid(new ValidationError { Identifier = "Name", ErrorMessage = ErrorMessage })
            .ShouldHaveValidationErrorWithIdentifier("Name");
    }

    [Fact]
    public void Conflict_ShouldBeFailure()
    {
        Result<string>.Conflict().ShouldBeFailure();
    }
}
