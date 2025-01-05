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
        Result.NotFound().ShouldBeNotFound().ShouldBeOfTypeBooleanAssertionConstraint();
    }

    [Fact]
    public void ConflictResultShouldBeFailureAsBooleanAssertionConstraint()
    {
        Result.Conflict().ShouldBeFailure().ShouldBeOfTypeBooleanAssertionConstraint();
    }

    [Fact]
    public void CriticalErrorResultShouldBeFailureAsBooleanAssertionConstraint()
    {
        Result.CriticalError().ShouldBeFailure().ShouldBeOfTypeBooleanAssertionConstraint();
    }

    [Fact]
    public void ErrorResultShouldBeFailureAsBooleanAssertionConstraint()
    {
        Result.Error().ShouldBeFailure().ShouldBeOfTypeBooleanAssertionConstraint();
    }

    [Fact]
    public void ForbiddenResultShouldBeFailureAsBooleanAssertionConstraint()
    {
        Result.Forbidden().ShouldBeFailure().ShouldBeOfTypeBooleanAssertionConstraint();
    }

    [Fact]
    public void InvalidResultShouldBeFailureAsBooleanAssertionConstraint()
    {
        Result.Invalid().ShouldBeFailure().ShouldBeOfTypeBooleanAssertionConstraint();
    }

    [Fact]
    public void NotFoundResultShouldBeFailureAsBooleanAssertionConstraint()
    {
        Result.NotFound().ShouldBeFailure().ShouldBeOfTypeBooleanAssertionConstraint();
    }

    [Fact]
    public void UnauthorizedResultShouldBeFailureAsBooleanAssertionConstraint()
    {
        Result.Unauthorized().ShouldBeFailure().ShouldBeOfTypeBooleanAssertionConstraint();
    }

    [Fact]
    public void UnavailableResultShouldBeFailureAsBooleanAssertionConstraint()
    {
        Result.Unavailable().ShouldBeFailure().ShouldBeOfTypeBooleanAssertionConstraint();
    }
}