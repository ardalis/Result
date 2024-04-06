using FluentAssertions;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace Ardalis.Result.UnitTests;

public class ResultVoidConstructor
{
    [Fact]
    public void InitializesSuccessResultWithConstructor()
    {
        var result = new Result();

        Assert.Null(result.Value);
        Assert.Equal(ResultStatus.Ok, result.Status);
    }

    [Fact]
    public void InitializesSuccessResultWithFactoryMethod()
    {
        var result = Result.Success();

        Assert.Null(result.Value);
        Assert.Equal(ResultStatus.Ok, result.Status);
    }

    [Fact]
    public void InitializesSuccessResultWithMessageWithFactoryMethod()
    {
        var message = "success";
        var result = Result.SuccessWithMessage(message);

        Assert.Null(result.Value);
        Assert.Equal(ResultStatus.Ok, result.Status);
        Assert.Equal(message, result.SuccessMessage);
    }

    [Theory]
    [InlineData("test1")]
    [InlineData("test1", "test2")]
    public void InitializesErrorResultWithFactoryMethod(params string[] errors)
    {
        var result = Result.Error(new ErrorList(errors, null));

        Assert.Null(result.Value);
        Assert.Equal(ResultStatus.Error, result.Status);

        if (errors == null) return;

        foreach (var error in errors)
        {
            result.Errors.Should().ContainEquivalentOf(error);
        }
    }

    [Fact]
    public void InitializesErrorResultWithCorrelationIdWithFactoryMethod()
    {
        var correlationId = "testId";
        var errors = new string[] { "Error 1", "Error 2" };
        var result = Result.Error(new ErrorList(errors, correlationId));

        Assert.Null(result.Value);
        Assert.Equal(ResultStatus.Error, result.Status);
        Assert.Equal(correlationId, result.CorrelationId);
        
        foreach (var error in errors)
        {
            result.Errors.Should().ContainEquivalentOf(error);
        }
    }

    [Fact]
    public void InitializesInvalidResultWithMultipleValidationErrorsWithFactoryMethod()
    {
        var validationErrors = new List<ValidationError>
            {
                new ValidationError
                {
                    Identifier = "name",
                    ErrorMessage = "Name is required"
                },
                new ValidationError
                {
                    Identifier = "postalCode",
                    ErrorMessage = "PostalCode cannot exceed 10 characters"
                }
            };

        var result = Result.Invalid(validationErrors);

        Assert.Null(result.Value);
        Assert.Equal(ResultStatus.Invalid, result.Status);

        result.ValidationErrors.Should().ContainEquivalentOf(new ValidationError { ErrorMessage = "Name is required", Identifier = "name" });
        result.ValidationErrors.Should().ContainEquivalentOf(new ValidationError { ErrorMessage = "PostalCode cannot exceed 10 characters", Identifier = "postalCode" });
    }

    [Fact]
    public void InitializesInvalidResultWithSingleValidationErrorWithFactoryMethod()
    {
        var validationError = new ValidationError
        {
            Identifier = "name",
            ErrorMessage = "Name is required"
        };

        var result = Result.Invalid(validationError);

        Assert.Null(result.Value);
        Assert.Equal(ResultStatus.Invalid, result.Status);

        result.ValidationErrors.Should().ContainEquivalentOf(new ValidationError { ErrorMessage = "Name is required", Identifier = "name" });
    }

    [Fact]
    public void InitializesNotFoundResultWithFactoryMethod()
    {
        var result = Result.NotFound();

        Assert.Null(result.Value);
        Assert.Equal(ResultStatus.NotFound, result.Status);
    }
    
    [Fact]
    public void InitializesNotFoundResultWithFactoryMethodWithErrors()
    {
        var errorMessage = "User Not Found";
        var result = Result.NotFound(errorMessage);

        result.Value.Should().BeNull();
        result.Status.Should().Be(ResultStatus.NotFound);
        result.Errors.Single().Should().Be(errorMessage);
    }

    [Fact]
    public void InitializesForbiddenResultWithFactoryMethod()
    {
        var result = Result.Forbidden();

        Assert.Null(result.Value);
        Assert.Equal(ResultStatus.Forbidden, result.Status);
    }

    [Fact]
    public void InitializesUnauthorizedResultWithFactoryMethod()
    {
        var result = Result.Unauthorized();

        Assert.Null(result.Value);
        Assert.Equal(ResultStatus.Unauthorized, result.Status);
    }
    
    [Fact]
    public void InitializesConflictResultWithFactoryMethod()
    {
        var result = Result.Conflict();

        result.Value.Should().BeNull();
        result.Status.Should().Be(ResultStatus.Conflict);
    }
    
    [Fact]
    public void InitializesConflictResultWithFactoryMethodWithErrors()
    {
        var errorMessage = "Some conflict";
        var result = Result.Conflict(errorMessage);

        result.Value.Should().BeNull();
        result.Status.Should().Be(ResultStatus.Conflict);
        result.Errors.Single().Should().Be(errorMessage);
    }

    [Fact]
    public void InitializeUnavailableResultWithFactoryMethodWithErrors()
    {
        var errorMessage = "Something unavailable";
        var result = Result.Unavailable(errorMessage);

        result.Value.Should().BeNull();
        result.Status.Should().Be(ResultStatus.Unavailable);
        result.Errors.Single().Should().Be(errorMessage);
    }
    
    [Fact]
    public void InitializesCriticalErrorResultWithFactoryMethodWithErrors()
    {
        var errorMessage = "Some critical error";
        var result = Result.CriticalError(errorMessage);

        result.Value.Should().BeNull();
        result.Status.Should().Be(ResultStatus.CriticalError);
        result.Errors.Single().Should().Be(errorMessage);
    }
}
