using FluentAssertions.Primitives;
using FluentAssertions;

namespace Ardalis.Result.FluentAssertions.UnitTests;

public static class AndConstraintExtensions
{

    public static AndConstraint<ObjectAssertions> ShouldBeOfTypeBooleanAssertionConstraint(this object obj)
    {
        return obj.Should().BeOfType(typeof(AndConstraint<BooleanAssertions>));
    }

    public static AndConstraint<ObjectAssertions> ShouldBeOfTypeAndConstraintObjectAssertion(this object obj)
    {
        return obj.Should().BeOfType(typeof(AndConstraint<ObjectAssertions>));
    }

    public static AndConstraint<ObjectAssertions> ShouldBeFailureOfTypeAndConstraintObjectAssertion(this Result result)
    {
        return result.ShouldBeFailure().ShouldBeOfTypeAndConstraintObjectAssertion();
    }

    public static AndConstraint<ObjectAssertions> ShouldBeFailureOfTypeAndConstraintObjectAssertion(this Result result, params string[] errorMessages)
    {
        return result.ShouldBeFailure(errorMessages).ShouldBeOfTypeAndConstraintObjectAssertion();
    }
}