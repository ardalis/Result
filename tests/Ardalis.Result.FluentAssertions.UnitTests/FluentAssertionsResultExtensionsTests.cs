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
    public void ConflictResultShouldBeFailureAsBooleanAssertionConstraint()
    {
        ShouldBeBooleanAssertionConstraint(Result.Conflict().ShouldBeFailure());
    }

    [Fact]
    public void CriticalErrorResultShouldBeFailureAsBooleanAssertionConstraint()
    {
        ShouldBeBooleanAssertionConstraint(Result.CriticalError().ShouldBeFailure());
    }

    [Fact]
    public void ErrorResultShouldBeFailureAsBooleanAssertionConstraint()
    {
        ShouldBeBooleanAssertionConstraint(Result.Error().ShouldBeFailure());
    }

    [Fact]
    public void ForbiddenResultShouldBeFailureAsBooleanAssertionConstraint()
    {
        ShouldBeBooleanAssertionConstraint(Result.Forbidden().ShouldBeFailure());
    }

    [Fact]
    public void InvalidResultShouldBeFailureAsBooleanAssertionConstraint()
    {
        ShouldBeBooleanAssertionConstraint(Result.Invalid().ShouldBeFailure());
    }

    [Fact]
    public void NotFoundResultShouldBeFailureAsBooleanAssertionConstraint()
    {
        ShouldBeBooleanAssertionConstraint(Result.NotFound().ShouldBeFailure());
    }

    [Fact]
    public void UnauthorizedResultShouldBeFailureAsBooleanAssertionConstraint()
    {
        ShouldBeBooleanAssertionConstraint(Result.Unauthorized().ShouldBeFailure());
    }

    [Fact]
    public void UnavailableResultShouldBeFailureAsBooleanAssertionConstraint()
    {
        ShouldBeBooleanAssertionConstraint(Result.Unavailable().ShouldBeFailure());
    }

    private static void ShouldBeBooleanAssertionConstraint(object obj)
    {
        obj.Should().BeOfType(typeof(AndConstraint<BooleanAssertions>));
    }
}