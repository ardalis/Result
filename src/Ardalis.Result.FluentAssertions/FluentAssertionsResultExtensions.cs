using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using FluentAssertions.Primitives;

namespace Ardalis.Result.FluentAssertions;

public static class FluentAssertionsResultExtensions
{
    private static readonly IReadOnlyCollection<ResultStatus> FailureResultStatus =
    [
        ResultStatus.Conflict,
        ResultStatus.CriticalError,
        ResultStatus.Error,
        ResultStatus.Forbidden,
        ResultStatus.Invalid,
        ResultStatus.NotFound,
        ResultStatus.Unauthorized,
        ResultStatus.Unavailable
    ];
    
    public static AndConstraint<ObjectAssertions> ShouldBeFailure(this Result result)
    {
        result.Status.Should().BeOneOf(FailureResultStatus);

        return new AndConstraint<ObjectAssertions>(result.Should());
    }

    public static AndConstraint<ObjectAssertions> ShouldBeConflict(this Result result)
    {
        return result.ShouldBeEquivalentTo(Result.Conflict());
    }

    public static AndConstraint<ObjectAssertions> ShouldBeConflict(this Result result, params string[] errorMessages)
    {
        return result.ShouldBeEquivalentTo(Result.Conflict(errorMessages));
    }

    public static AndConstraint<ObjectAssertions> ShouldBeCriticalError(this Result result)
    {
        return result.ShouldBeEquivalentTo(Result.CriticalError());
    }

    public static AndConstraint<ObjectAssertions> ShouldBeCriticalError(this Result result, params string[] errorMessages)
    {
        return result.ShouldBeEquivalentTo(Result.CriticalError(errorMessages));
    }

    public static AndConstraint<ObjectAssertions> ShouldBeError(this Result result)
    {
        return result.ShouldBeEquivalentTo(Result.Error());
    }

    public static AndConstraint<ObjectAssertions> ShouldBeError(this Result result, string errorMessage)
    {
        return result.ShouldBeEquivalentTo(Result.Error(errorMessage));
    }

    public static AndConstraint<ObjectAssertions> ShouldBeError(this Result result, ErrorList errorList)
    {
        return result.ShouldBeEquivalentTo(Result.Error(errorList));
    }

    public static AndConstraint<ObjectAssertions> ShouldBeError(this Result result, IEnumerable<string> errorMessages, string? correlationId)
    {
        return result.ShouldBeEquivalentTo(Result.Error(new ErrorList(errorMessages, correlationId)));
    }

    public static AndConstraint<ObjectAssertions> ShouldBeForbidden(this Result result)
    {
        return result.ShouldBeEquivalentTo(Result.Forbidden());
    }

    public static AndConstraint<ObjectAssertions> ShouldBeForbidden(this Result result, params string[] errorMessages)
    {
        return result.ShouldBeEquivalentTo(Result.Forbidden(errorMessages));
    }

    public static AndConstraint<ObjectAssertions> ShouldBeInvalid(this Result result)
    {
        result.Status.Should().Be(ResultStatus.Invalid);

        return new AndConstraint<ObjectAssertions>(result.Should());
    }

    public static AndConstraint<ObjectAssertions> ShouldBeInvalid(this Result result, params ValidationError[] validationErrors)
    {
        return result.ShouldBeEquivalentTo(Result.Invalid(validationErrors));
    }

    public static AndConstraint<ObjectAssertions> ShouldBeNotFound(this Result result)
    {
        return result.ShouldBeEquivalentTo(Result.NotFound());
    }

    public static AndConstraint<ObjectAssertions> ShouldBeNotFound(this Result result, params string[] errorMessages)
    {
        return result.ShouldBeEquivalentTo(Result.NotFound(errorMessages));
    }

    public static AndConstraint<ObjectAssertions> ShouldBeUnauthorized(this Result result)
    {
        return result.ShouldBeEquivalentTo(Result.Unauthorized());
    }

    public static AndConstraint<ObjectAssertions> ShouldBeUnauthorized(this Result result, params string[] errorMessages)
    {
        return result.ShouldBeEquivalentTo(Result.Unauthorized(errorMessages));
    }

    public static AndConstraint<ObjectAssertions> ShouldBeUnavailable(this Result result)
    {
        return result.ShouldBeEquivalentTo(Result.Unavailable());
    }

    public static AndConstraint<ObjectAssertions> ShouldBeUnavailable(this Result result, params string[] errorMessages)
    {
        return result.ShouldBeEquivalentTo(Result.Unavailable(errorMessages));
    }

    public static AndConstraint<ObjectAssertions> ShouldBeFailure(this Result result, params string[] errorMessages)
    {
        var andConstraint = result.ShouldBeFailure();

        result.Errors.Should().BeEquivalentTo(errorMessages);

        return andConstraint;
    }
    
    public static AndConstraint<ObjectAssertions> ShouldHaveValidationErrorWithCode(this Result result, string errorCode)
    {
        var andConstraint = result.ShouldBeInvalid();

        result.ValidationErrors.Count().Should().BePositive();

        result.ValidationErrors.First().ErrorCode.Should().Be(errorCode);

        return andConstraint;
    }
    
    public static AndConstraint<ObjectAssertions> ShouldHaveValidationErrorWithIdentifier(this Result result, string identifier)
    {
        var andConstraint = result.ShouldBeInvalid();

        result.ValidationErrors.Count().Should().BePositive();

        result.ValidationErrors.First().Identifier.Should().Be(identifier);

        return andConstraint;
    }

    private static AndConstraint<ObjectAssertions> ShouldBeEquivalentTo(this Result result, Result assertingResult)
    {
        return result.Should().BeEquivalentTo(assertingResult);
    }
}