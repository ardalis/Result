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


    //ShouldBeFailure
    
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

    //ShouldBEFailureWithMessage

    [Fact]
    public void ConflictResultWithMessage_ShouldBeNotFoundWithMessage_AsAndConstraintObjectAssertion()
    {
        Result.Conflict(ErrorMessage).ShouldBeFailureOfTypeAndConstraintObjectAssertion(ErrorMessage);
    }
    
    [Fact]
    public void CriticalErrorResultWithMessage_ShouldBeNotFoundWithMessage_AsAndConstraintObjectAssertion()
    {
        Result.CriticalError(ErrorMessage).ShouldBeFailureOfTypeAndConstraintObjectAssertion(ErrorMessage);
    }
    
    [Fact]
    public void ErrorResultWithMessage_ShouldBeNotFoundWithMessage_AsAndConstraintObjectAssertion()
    {
        Result.Error(ErrorMessage).ShouldBeFailureOfTypeAndConstraintObjectAssertion(ErrorMessage);
    }
    
    [Fact]
    public void ForbiddenResultWithMessage_ShouldBeNotFoundWithMessage_AsAndConstraintObjectAssertion()
    {
        Result.Forbidden(ErrorMessage).ShouldBeFailureOfTypeAndConstraintObjectAssertion(ErrorMessage);
    }
    
    //[Fact]
    //public void InvalidResultWithMessage_ShouldBeNotFoundWithMessage_AsAndConstraintObjectAssertion()
    //{
    //    Result.Invalid(ErrorMessage).ShouldBeFailureOfTypeAndConstraintObjectAssertion(ErrorMessage);
    //}
    
    [Fact]
    public void NotFoundResultWithMessage_ShouldBeNotFoundWithMessage_AsAndConstraintObjectAssertion()
    {
        Result.NotFound(ErrorMessage).ShouldBeFailureOfTypeAndConstraintObjectAssertion(ErrorMessage);
    }
    
    [Fact]
    public void UnauthorizedResultWithMessage_ShouldBeNotFoundWithMessage_AsAndConstraintObjectAssertion()
    {
        Result.Unauthorized(ErrorMessage).ShouldBeFailureOfTypeAndConstraintObjectAssertion(ErrorMessage);
    }

    [Fact]
    public void UnavailableResultWithMessage_ShouldBeNotFoundWithMessage_AsAndConstraintObjectAssertion()
    {
        Result.Unavailable(ErrorMessage).ShouldBeFailureOfTypeAndConstraintObjectAssertion(ErrorMessage);
    }
}