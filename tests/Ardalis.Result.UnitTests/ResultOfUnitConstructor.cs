using FluentAssertions;
using System;
using System.Collections.Generic;
using Xunit;

namespace Ardalis.Result.UnitTests;

public class ResultOfUnitConstructor
{
    [Fact]
    public void InitializesSuccessResultWithConstructor()
    {
        var result = new Result();

        Assert.Equal(Result.Unit.Value, result.Value);
        Assert.Equal(ResultStatus.Ok, result.Status);
    }

    [Fact]
    public void InitializesSuccessResultWithFactoryMethod()
    {
        var result = Result.Success();

        Assert.Equal(Result.Unit.Value, result.Value);
        Assert.Equal(ResultStatus.Ok, result.Status);
    }

    [Fact]
    public void InitializesSuccessResultWithMessageWithFactoryMethod()
    {
        var message = "success";
        var result = Result.SuccessWithMessage(message);

        Assert.Equal(Result.Unit.Value, result.Value);
        Assert.Equal(ResultStatus.Ok, result.Status);
        Assert.Equal(message, result.SuccessMessage);
    }

    [Theory]
    [InlineData("test1")]
    [InlineData("test1", "test2")]
    public void InitializesErrorResultWithFactoryMethod(params string[] errors)
    {
        var result = Result.Error(errors);

        Assert.Equal(Result.Unit.Value, result.Value);
        Assert.Equal(ResultStatus.Error, result.Status);

        if (errors == null) return;

        foreach (var error in errors)
        {
            result.Errors.Should().ContainEquivalentOf(error);
        }
    }

    [Fact]
    public void InitializesInvalidResultWithFactoryMethod()
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

        Assert.Equal(Result.Unit.Value, result.Value);
        Assert.Equal(ResultStatus.Invalid, result.Status);

        result.ValidationErrors.Should().ContainEquivalentOf(new ValidationError { ErrorMessage = "Name is required", Identifier = "name" });
        result.ValidationErrors.Should().ContainEquivalentOf(new ValidationError { ErrorMessage = "PostalCode cannot exceed 10 characters", Identifier = "postalCode" });
    }

    [Fact]
    public void InitializesNotFoundResultWithFactoryMethod()
    {
        var result = Result.NotFound();

        Assert.Equal(Result.Unit.Value, result.Value);
        Assert.Equal(ResultStatus.NotFound, result.Status);
    }

    [Fact]
    public void InitializesForbiddenResultWithFactoryMethod()
    {
        var result = Result.Forbidden();

        Assert.Equal(Result.Unit.Value, result.Value);
        Assert.Equal(ResultStatus.Forbidden, result.Status);
    }

    [Fact]
    public void InitializesUnauthorizedResultWithFactoryMethod()
    {
        var result = Result.Unauthorized();

        Assert.Equal(Result.Unit.Value, result.Value);
        Assert.Equal(ResultStatus.Unauthorized, result.Status);
    }
}
