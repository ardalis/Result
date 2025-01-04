using FluentAssertions;
using FluentAssertions.Primitives;
using Xunit;
using Xunit.Sdk;

namespace Ardalis.Result.FluentAssertions.UnitTests;

public class FluentAssertionsResultExtensionsTests
{
    private const string ErrorMessage = "Error message"; 
    
    [Fact]
    public void ResultErrorShouldBeError()
    {
        Result.Error().ShouldBeError().Should().BeOfType(typeof(AndConstraint<BooleanAssertions>));
    }

    [Fact]
    public void ResultErrorShouldBeErrorWithMessage()
    {
        Result.Error(ErrorMessage).ShouldBeError(ErrorMessage).Should().BeOfType(typeof(AndConstraint<BooleanAssertions>));
    }
}