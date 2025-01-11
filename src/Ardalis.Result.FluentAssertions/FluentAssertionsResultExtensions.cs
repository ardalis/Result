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
        result.IsSuccess.Should().BeFalse();

        return new AndConstraint<ObjectAssertions>(result.Should());
    }

    public static AndConstraint<ObjectAssertions> ShouldBeConflict(this Result result)
    {
        result.Status.Should().Be(ResultStatus.Conflict);

        return new AndConstraint<ObjectAssertions>(result.Should());
    }

    public static AndConstraint<ObjectAssertions> ShouldBeConflict(this Result result, params string[] errorMessages)
    {
        return result.ShouldBeEquivalentTo(Result.Conflict(errorMessages));
    }

    public static AndConstraint<ObjectAssertions> ShouldBeCriticalError(this Result result)
    {
        result.Status.Should().Be(ResultStatus.CriticalError);

        return new AndConstraint<ObjectAssertions>(result.Should());
    }

    public static AndConstraint<ObjectAssertions> ShouldBeCriticalError(this Result result, params string[] errorMessages)
    {
        return result.ShouldBeEquivalentTo(Result.CriticalError(errorMessages));
    }

    public static AndConstraint<ObjectAssertions> ShouldBeError(this Result result)
    {
        result.Status.Should().Be(ResultStatus.Error);

        return new AndConstraint<ObjectAssertions>(result.Should());
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
        result.Status.Should().Be(ResultStatus.Forbidden);

        return new AndConstraint<ObjectAssertions>(result.Should());
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
        result.Status.Should().Be(ResultStatus.NotFound);

        return new AndConstraint<ObjectAssertions>(result.Should());
    }

    public static AndConstraint<ObjectAssertions> ShouldBeNotFound(this Result result, params string[] errorMessages)
    {
        return result.ShouldBeEquivalentTo(Result.NotFound(errorMessages));
    }

    public static AndConstraint<ObjectAssertions> ShouldBeUnauthorized(this Result result)
    {
        result.Status.Should().Be(ResultStatus.Unauthorized);

        return new AndConstraint<ObjectAssertions>(result.Should());
    }

    public static AndConstraint<ObjectAssertions> ShouldBeUnauthorized(this Result result, params string[] errorMessages)
    {
        return result.ShouldBeEquivalentTo(Result.Unauthorized(errorMessages));
    }

    public static AndConstraint<ObjectAssertions> ShouldBeUnavailable(this Result result)
    {
        result.Status.Should().Be(ResultStatus.Unavailable);

        return new AndConstraint<ObjectAssertions>(result.Should());
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

        result.ValidationErrors.Should().Satisfy(validationError => validationError.ErrorCode == errorCode);

        return andConstraint;
    }
    
    public static AndConstraint<ObjectAssertions> ShouldHaveValidationErrorWithIdentifier(this Result result, string identifier)
    {
        var andConstraint = result.ShouldBeInvalid();

        result.ValidationErrors.Count().Should().BePositive();

        result.ValidationErrors.Should().Satisfy(validationError => validationError.Identifier == identifier);

        return andConstraint;
    }
    
    public static AndConstraint<ObjectAssertions> ShouldHaveValidationErrorWithMessage(this Result result, string errorMessage)
    {
        var andConstraint = result.ShouldBeInvalid();

        result.ValidationErrors.Count().Should().BePositive();

        result.ValidationErrors.Should().Satisfy(validationError => validationError.ErrorMessage == errorMessage);

        return andConstraint;
    }
    
    public static AndConstraint<ObjectAssertions> ShouldHaveValidationErrorWithSeverity(this Result result, ValidationSeverity severity)
    {
        var andConstraint = result.ShouldBeInvalid();

        result.ValidationErrors.Count().Should().BePositive();

        result.ValidationErrors.Should().Satisfy(validationError => validationError.Severity == severity);

        return andConstraint;
    }

    public static AndConstraint<ObjectAssertions> ShouldBeSuccess(this Result result)
    {
        result.IsSuccess.Should().BeTrue();

        return new AndConstraint<ObjectAssertions>(result.Should());
    }
    
    public static AndConstraint<ObjectAssertions> ShouldBeSuccessWithMessage(this Result result, string successMessage)
    {
        return result.ShouldBeEquivalentTo(Result.SuccessWithMessage(successMessage));
    }
    
    private static AndConstraint<ObjectAssertions> ShouldBeEquivalentTo(this Result result, Result assertingResult)
    {
        return result.Should().BeEquivalentTo(assertingResult);
    }
}