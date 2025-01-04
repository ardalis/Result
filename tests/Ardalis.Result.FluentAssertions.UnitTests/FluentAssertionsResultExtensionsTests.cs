using FluentAssertions;
using FluentAssertions.Primitives;
using Xunit;

namespace Ardalis.Result.FluentAssertions.UnitTests;

public class FluentAssertionsResultExtensionsTests
{
    [Fact]
    public void ResultErrorShouldBeError()
    {
        Result.Error().ShouldBeError().Should().BeOfType(typeof(AndConstraint<BooleanAssertions>));
    }

    [Fact]
    public void ResultErrorShouldBeErrorWithMessage()
    {
        Result.Error("Error message").ShouldBeError("Error message").Should().BeOfType(typeof(AndConstraint<BooleanAssertions>));
    }
}