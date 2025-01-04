using FluentAssertions;
using FluentAssertions.Primitives;

namespace Ardalis.Result.FluentAssertions;

public static class FluentAssertionsResultExtensions
{
    public static AndConstraint<BooleanAssertions> ShouldBeError(this Result result)
    {
        return result.IsError().Should().BeTrue();
    }
}