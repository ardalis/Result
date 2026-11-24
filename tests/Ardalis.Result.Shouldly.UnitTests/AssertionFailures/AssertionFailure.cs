using Shouldly;
using Xunit;
using static Ardalis.Result.Shouldly.UnitTests.Utils.Constants;

namespace Ardalis.Result.Shouldly.UnitTests.AssertionFailures;

// Each assertion must throw when the result doesn't match, or the tests using it can't fail.
public class AssertionFailure
{
    [Fact]
    public void ShouldBeSuccess_ThrowsForFailure()
    {
        Should.Throw<ShouldAssertException>(() => Result.Error().ShouldBeSuccess());
    }

    [Fact]
    public void ShouldBeFailure_ThrowsForSuccess()
    {
        Should.Throw<ShouldAssertException>(() => Result<int>.Success(1).ShouldBeFailure());
    }

    [Fact]
    public void ShouldBeNotFound_ThrowsForDifferentStatus()
    {
        Should.Throw<ShouldAssertException>(() => Result<int>.Conflict().ShouldBeNotFound());
    }

    [Fact]
    public void ShouldBeNotFound_ThrowsForDifferentErrorMessages()
    {
        Should.Throw<ShouldAssertException>(() => Result.NotFound(ErrorMessage).ShouldBeNotFound("OTHER_MESSAGE"));
    }

    [Fact]
    public void ShouldBeError_ThrowsForDifferentCorrelationId()
    {
        var actual = Result.Error(new ErrorList(new[] { ErrorMessage }, "CORRELATION_ID"));

        Should.Throw<ShouldAssertException>(
            () => actual.ShouldBeError(new ErrorList(new[] { ErrorMessage }, "OTHER_ID")));
    }

    [Fact]
    public void ShouldHaveValidationErrorWithCode_ThrowsWhenCodeMissing()
    {
        var result = Result.Invalid(new ValidationError { ErrorCode = "CODE_1" });

        Should.Throw<ShouldAssertException>(() => result.ShouldHaveValidationErrorWithCode("CODE_2"));
    }

    [Fact]
    public void ShouldBeCreatedWithLocation_ThrowsForDifferentLocation()
    {
        Should.Throw<ShouldAssertException>(
            () => Result<int>.Created(1, "LOCATION").ShouldBeCreatedWithLocation("OTHER_LOCATION"));
    }
}
