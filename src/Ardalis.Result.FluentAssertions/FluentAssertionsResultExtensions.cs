using System.Collections.Generic;
using FluentAssertions;
using FluentAssertions.Primitives;

namespace Ardalis.Result.FluentAssertions;

public static class FluentAssertionsResultExtensions
{
    public static AndConstraint<BooleanAssertions> ShouldBeFailure(this Result result)
    {
        var res = new List<ResultStatus>()
        {
            ResultStatus.Conflict,
            ResultStatus.CriticalError,
            ResultStatus.Error,
            ResultStatus.Forbidden,
            ResultStatus.Invalid,
            ResultStatus.NotFound,
            ResultStatus.Unauthorized,
            ResultStatus.Unavailable
        };
        
        result.Status.Should().BeOneOf(res);

        return new AndConstraint<BooleanAssertions>(new BooleanAssertions(true));
    }
    
    public static AndConstraint<BooleanAssertions> ShouldBeNotFound(this Result result)
    {
        return result.IsNotFound().Should().BeTrue();
    }
    
    public static AndConstraint<BooleanAssertions> ShouldBeError(this Result result)
    {
        return result.IsError().Should().BeTrue();
    }

    public static AndConstraint<BooleanAssertions> ShouldBeError(this Result result, params string[] errorMessages)
    {
        result.ShouldBeError();

        result.Errors.Should().BeEquivalentTo(errorMessages);

        return new AndConstraint<BooleanAssertions>(new BooleanAssertions(true));
    }
}