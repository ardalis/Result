using System.Collections.Generic;
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

    public static AndConstraint<ObjectAssertions> ShoudBeInvalid(this Result result)
    {
        return result.ShouldBeEquivalentTo(Result.Invalid());
    }

    public static AndConstraint<ObjectAssertions> ShouldBeFailure(this Result result, params string[] errorMessages)
    {
        var andConstraint = result.ShouldBeFailure();

        result.Errors.Should().BeEquivalentTo(errorMessages);

        return andConstraint;
    }

    private static AndConstraint<ObjectAssertions> ShouldBeEquivalentTo(this Result result, Result assertingResult)
    {
        return result.Should().BeEquivalentTo(assertingResult);
    }
}