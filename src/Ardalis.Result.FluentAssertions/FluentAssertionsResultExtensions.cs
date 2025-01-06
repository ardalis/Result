using System.Collections.Generic;
using FluentAssertions;
using FluentAssertions.Collections;
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
        return result.Should().BeEquivalentTo(Result.Conflict());
    }

    public static AndConstraint<ObjectAssertions> ShouldBeConflict(this Result result, params string[] errorMessages)
    {
        return result.Should().BeEquivalentTo(Result.Conflict(errorMessages));
    }

    public static AndConstraint<ObjectAssertions> ShouldBeCriticalError(this Result result)
    {
        return result.Should().BeEquivalentTo(Result.CriticalError());
    }

    public static AndConstraint<ObjectAssertions> ShouldBeCriticalError(this Result result, params string[] errorMessages)
    {
        return result.Should().BeEquivalentTo(Result.CriticalError(errorMessages));
    }

    public static AndConstraint<ObjectAssertions> ShouldBeError(this Result result)
    {
        return result.Should().BeEquivalentTo(Result.Error());
    }

    public static AndConstraint<ObjectAssertions> ShouldBeError(this Result result, string errorMessage)
    {
        return result.Should().BeEquivalentTo(Result.Error(errorMessage));
    }

    public static AndConstraint<ObjectAssertions> ShouldBeError(this Result result, ErrorList errorList)
    {
        return result.Should().BeEquivalentTo(Result.Error(errorList));
    }

    public static AndConstraint<ObjectAssertions> ShouldBeError(this Result result, IEnumerable<string> errorMessages, string? correlationId)
    {
        return result.Should().BeEquivalentTo(Result.Error(new ErrorList(errorMessages, correlationId)));
    }



    public static AndConstraint<ObjectAssertions> ShouldBeFailure(this Result result, params string[] errorMessages)
    {
        result.ShouldBeFailure();

        result.Errors.Should().BeEquivalentTo(errorMessages);

        return new AndConstraint<ObjectAssertions>(result.Should());
    }
}