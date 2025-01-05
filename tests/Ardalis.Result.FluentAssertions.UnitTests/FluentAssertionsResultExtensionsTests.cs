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
        Result.Error().ShouldBeError().ShouldBeOfTypeBooleanAssertionConstraint();
    }

    [Fact]
    public void ErrorResultShouldBeErrorWithMessageAsBooleanAssertionConstraint()
    {
        Result.Error(ErrorMessage).ShouldBeError(ErrorMessage).ShouldBeOfTypeBooleanAssertionConstraint();
    }

    [Fact]
    public void NotFoundResultShouldBeNotFoundAsBooleanAssertionConstraint()
    {
        Result.NotFound().ShouldBeFailureOfTypeBooleanAssertionConstraint();
    }

    [Fact]
    public void ConflictResultShouldBeFailureAsBooleanAssertionConstraint()
    {
        Result.Conflict().ShouldBeFailureOfTypeBooleanAssertionConstraint();
    }

    [Fact]
    public void CriticalErrorResultShouldBeFailureAsBooleanAssertionConstraint()
    {
        Result.CriticalError().ShouldBeFailureOfTypeBooleanAssertionConstraint();
    }

    [Fact]
    public void ErrorResultShouldBeFailureAsBooleanAssertionConstraint()
    {
        Result.Error().ShouldBeFailureOfTypeBooleanAssertionConstraint();
    }

    [Fact]
    public void ForbiddenResultShouldBeFailureAsBooleanAssertionConstraint()
    {
        Result.Forbidden().ShouldBeFailureOfTypeBooleanAssertionConstraint();
    }

    [Fact]
    public void InvalidResultShouldBeFailureAsBooleanAssertionConstraint()
    {
        Result.Invalid().ShouldBeFailureOfTypeBooleanAssertionConstraint();
    }

    [Fact]
    public void NotFoundResultShouldBeFailureAsBooleanAssertionConstraint()
    {
        Result.NotFound().ShouldBeFailureOfTypeBooleanAssertionConstraint();
    }

    [Fact]
    public void UnauthorizedResultShouldBeFailureAsBooleanAssertionConstraint()
    {
        Result.Unauthorized().ShouldBeFailureOfTypeBooleanAssertionConstraint();
    }

    [Fact]
    public void UnavailableResultShouldBeFailureAsBooleanAssertionConstraint()
    {
        Result.Unavailable().ShouldBeFailureOfTypeBooleanAssertionConstraint();
    }
}