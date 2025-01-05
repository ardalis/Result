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
        Result.Error().ShouldBeError().ShouldBeOfTypeAndConstraintObjectAssertion();
    }

    [Fact]
    public void ErrorResultShouldBeErrorWithMessageAsObjectAssertionConstraint()
    {
        Result.Error(ErrorMessage).ShouldBeError(ErrorMessage).ShouldBeOfTypeAndConstraintObjectAssertion();
    }

    [Fact]
    public void NotFoundResultShouldBeNotFoundAsBooleanAssertionConstraint()
    {
        Result.NotFound().ShouldBeFailureOfTypeAndConstraintObjectAssertion();
    }

    [Fact]
    public void ConflictResultShouldBeFailureAsBooleanAssertionConstraint()
    {
        Result.Conflict().ShouldBeFailureOfTypeAndConstraintObjectAssertion();
    }

    [Fact]
    public void CriticalErrorResultShouldBeFailureAsBooleanAssertionConstraint()
    {
        Result.CriticalError().ShouldBeFailureOfTypeAndConstraintObjectAssertion();
    }

    [Fact]
    public void ErrorResultShouldBeFailureAsBooleanAssertionConstraint()
    {
        Result.Error().ShouldBeFailureOfTypeAndConstraintObjectAssertion();
    }

    [Fact]
    public void ForbiddenResultShouldBeFailureAsBooleanAssertionConstraint()
    {
        Result.Forbidden().ShouldBeFailureOfTypeAndConstraintObjectAssertion();
    }

    [Fact]
    public void InvalidResultShouldBeFailureAsBooleanAssertionConstraint()
    {
        Result.Invalid().ShouldBeFailureOfTypeAndConstraintObjectAssertion();
    }

    [Fact]
    public void NotFoundResultShouldBeFailureAsBooleanAssertionConstraint()
    {
        Result.NotFound().ShouldBeFailureOfTypeAndConstraintObjectAssertion();
    }

    [Fact]
    public void UnauthorizedResultShouldBeFailureAsBooleanAssertionConstraint()
    {
        Result.Unauthorized().ShouldBeFailureOfTypeAndConstraintObjectAssertion();
    }

    [Fact]
    public void UnavailableResultShouldBeFailureAsBooleanAssertionConstraint()
    {
        Result.Unavailable().ShouldBeFailureOfTypeAndConstraintObjectAssertion();
    }
}