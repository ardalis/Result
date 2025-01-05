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

    public static AndConstraint<ObjectAssertions> ShouldBeFailure(this Result result, params string[] errorMessages)
    {
        result.ShouldBeFailure();

        result.Errors.Should().BeEquivalentTo(errorMessages);

        return new AndConstraint<ObjectAssertions>(result.Should());
    }
}