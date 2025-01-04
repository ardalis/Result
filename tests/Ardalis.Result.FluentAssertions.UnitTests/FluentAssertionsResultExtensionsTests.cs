using FluentAssertions;
using FluentAssertions.Primitives;
using Xunit;
using Xunit.Sdk;

namespace Ardalis.Result.FluentAssertions.UnitTests;

public class FluentAssertionsResultExtensionsTests
{
    private const string ErrorMessage = "Error message"; 
    
    [Fact]
    public void ErrorResultShouldBeErrorAsBooleanAssertionConstraint()
    {
        ShouldBeBooleanAssertionConstraint(Result.Error().ShouldBeError());
    }

    [Fact]
    public void ErrorResultShouldBeErrorWithMessageAsBooleanAssertionConstraint()
    {
        ShouldBeBooleanAssertionConstraint(Result.Error(ErrorMessage).ShouldBeError(ErrorMessage));
    }

    [Fact]
    public void NotFoundResultShouldBeNotFoundAsBooleanAssertionConstraint()
    {
        ShouldBeBooleanAssertionConstraint(Result.NotFound().ShouldBeNotFound());
    }

    [Fact]
    public void NotFoundResultShouldBeFailureAsBooleanAssertionConstraint()
    {
        ShouldBeBooleanAssertionConstraint(Result.NotFound().ShouldBeFailure());
    }

    private static void ShouldBeBooleanAssertionConstraint(object obj)
    {
        obj.Should().BeOfType(typeof(AndConstraint<BooleanAssertions>));
    }
}