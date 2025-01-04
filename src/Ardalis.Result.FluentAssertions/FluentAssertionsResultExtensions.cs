using FluentAssertions;
using FluentAssertions.Primitives;

namespace Ardalis.Result.FluentAssertions;

public static class FluentAssertionsResultExtensions
{
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