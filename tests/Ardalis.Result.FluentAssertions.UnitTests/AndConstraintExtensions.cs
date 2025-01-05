using FluentAssertions.Primitives;
using FluentAssertions;

namespace Ardalis.Result.FluentAssertions.UnitTests;

public static class AndConstraintExtensions
{

    public static AndConstraint<ObjectAssertions> ShouldBeOfTypeBooleanAssertionConstraint(this object obj)
    {
        return obj.Should().BeOfType(typeof(AndConstraint<BooleanAssertions>));
    }
}