using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace Ardalis.Result.UnitTests;

public class ResultConstructor
{
    private class TestObject
    {
    }

    [Fact]
    public void InitializesStronglyTypedStringValue()
    {
        string expectedString = "test string";
        var result = new Result<string>(expectedString);

        Assert.Equal(expectedString, result.Value);
    }

    [Fact]
    public void InitializesStronglyTypedIntValue()
    {
        int expectedInt = 123;
        var result = new Result<int>(expectedInt);

        Assert.Equal(expectedInt, result.Value);
    }

    [Fact]
    public void InitializesStronglyTypedObjectValue()
    {
        var expectedObject = new TestObject();
        var result = new Result<TestObject>(expectedObject);

        Assert.Equal(expectedObject, result.Value);
    }

    [Fact]
    public void InitializesValueToNullGivenNullConstructorArgument()
    {
        var result = new Result<object>(null);

        Assert.Null(result.Value);
    }

    [Theory]
    [InlineData(null)]
    [InlineData(123)]
    [InlineData("test value")]
    public void InitializesStatusToOkGivenValue(object value)
    {
        var result = new Result<object>(value);

        Assert.Equal(ResultStatus.Ok, result.Status);
    }

    [Theory]
    [InlineData(null)]
    [InlineData(123)]
    [InlineData("test value")]
    public void InitializesValueUsingFactoryMethodAndSetsStatusToOk(object value)
    {
        var result = Result<object>.Success(value);

        Assert.Equal(ResultStatus.Ok, result.Status);
        Assert.Equal(value, result.Value);
    }

    [Theory]
    [InlineData(null)]
    [InlineData(123)]
    [InlineData("test value")]
    public void InitializesValueUsingGenericFactoryMethodAndSetsStatusToOk(object value)
    {
        var result = Result.Success(value);

        Assert.Equal(ResultStatus.Ok, result.Status);
        Assert.Equal(value, result.Value);
    }

    [Theory]
    [InlineData(null)]
    [InlineData(123)]
    [InlineData("test value")]
    public void InitializesValueUsingGenericFactoryMethodAndSetsStatusToOkWithMessage(object value)
    {
        var message = "success";
        var result = Result.Success(value, message);

        Assert.Equal(ResultStatus.Ok, result.Status);
        Assert.Equal(value, result.Value);
        Assert.Equal(message, result.SuccessMessage);
    }

    [Theory]
    [InlineData(null)]
    [InlineData(123)]
    [InlineData("test value")]
    public void InitializesStatusToCreatedAndSetLocationGivenCreatedFactoryCall(object value)
    {
        string location = "https://github.com/ardalis/Result";
        var result = Result<object>.Created(value, location);

        Assert.Equal(ResultStatus.Created, result.Status);
        Assert.Equal(location, result.Location);
        Assert.True(result.IsSuccess);
    }

    [Theory]
    [InlineData(null)]
    [InlineData(123)]
    [InlineData("test value")]
    public void InitializesStatusToCreatedGivenCreatedFactoryCall(object value)
    {
        var result = Result<object>.Created(value);

        Assert.Equal(ResultStatus.Created, result.Status);
        Assert.Equal(result.Location, string.Empty);
        Assert.True(result.IsSuccess);
    }

    [Fact]
    public void InitializesStatusToErrorGivenErrorFactoryCall()
    {
        var result = Result<object>.Error();

        Assert.Equal(ResultStatus.Error, result.Status);
    }

    [Fact]
    public void InitializesStatusToErrorGivenErrorFactoryCallWithSimpleMessage()
    {
        string errorMessage = Guid.NewGuid().ToString();
        var result = Result<object>.Error(errorMessage);

        Assert.Equal(ResultStatus.Error, result.Status);
        Assert.Equal(errorMessage, result.Errors.Single());
    }

    [Fact]
    public void InitializesStatusToErrorAndSetsErrorMessageGivenErrorFactoryCall()
    {
        string errorMessage = "Something bad happened.";
        string correlationId = Guid.NewGuid().ToString();
        ErrorList errors = new(new[] { errorMessage }, correlationId);
        var result = Result<object>.Error(errors);

        Assert.Equal(ResultStatus.Error, result.Status);
        Assert.Equal(errorMessage, result.Errors.Single());
        Assert.Equal(correlationId, result.CorrelationId);
    }

    [Fact]
    public void InitializesStatusToErrorAndSetsErrorMessageGivenErrorFactoryCallWithoutCorrelationId()
    {
        string errorMessage = "Something bad happened.";
        ErrorList errors = new(new[] { errorMessage });
        var result = Result<object>.Error(errors);

        Assert.Equal(ResultStatus.Error, result.Status);
        Assert.Equal(errorMessage, result.Errors.Single());
    }

    [Fact]
    public void InitializesStatusToInvalidAndSetsErrorMessagesGivenInvalidFactoryCall()
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
        // TODO: Support duplicates of the same key with multiple errors
        var result = Result<object>.Invalid(validationErrors);

        result.Status.Should().Be(ResultStatus.Invalid);
        result.ValidationErrors.Should().ContainEquivalentOf(new ValidationError { ErrorMessage = "Name is required", Identifier = "name" });
        result.ValidationErrors.Should().ContainEquivalentOf(new ValidationError { ErrorMessage = "PostalCode cannot exceed 10 characters", Identifier = "postalCode" });
    }

    [Fact]
    public void InitializesStatusToNotFoundGivenNotFoundFactoryCall()
    {
        var result = Result<object>.NotFound();

        Assert.Equal(ResultStatus.NotFound, result.Status);
        result.Errors.Should().BeEmpty();
    }

    [Fact]
    public void InitializesStatusToNotFoundGivenNotFoundFactoryCallWithString()
    {
        var errorMessage = "User Not Found";
        var result = Result<object>.NotFound(errorMessage);

        Assert.Equal(ResultStatus.NotFound, result.Status);
        Assert.Equal(errorMessage, result.Errors.First());
    }

    [Fact]
    public void InitializesStatusToConflictGivenConflictFactoryCall()
    {
        var result = Result<object>.Conflict();

        result.Status.Should().Be(ResultStatus.Conflict);
        result.Errors.Should().BeEmpty();
    }

    [Fact]
    public void InitializesStatusToConflictGivenConflictFactoryCallWithString()
    {
        var errorMessage = "Some conflict";
        var result = Result<object>.Conflict(errorMessage);

        result.Status.Should().Be(ResultStatus.Conflict);
        result.Errors.Single().Should().Be(errorMessage);
    }

    [Fact]
    public void InitializesStatusToForbiddenGivenForbiddenFactoryCall()
    {
        var result = Result<object>.Forbidden();

        Assert.Equal(ResultStatus.Forbidden, result.Status);
    }

    [Fact]
    public void InitializesStatusToUnavailableGivenUnavailableFactoryCallWithString()
    {
        var errorMessage = "Service Unavailable";
        var result = Result<object>.Unavailable(errorMessage);

        Assert.Equal(ResultStatus.Unavailable, result.Status);
        Assert.Equal(errorMessage, result.Errors.First());
    }


    [Fact]
    public void InitializedIsSuccessTrueForSuccessFactoryCall()
    {
        var result = Result<object>.Success(new object());

        Assert.True(result.IsSuccess);
    }

    [Fact]
    public void InitializedIsSuccessFalseForErrorFactoryCall()
    {
        var result = Result<object>.Error();

        Assert.False(result.IsSuccess);
    }

    [Fact]
    public void InitializedIsSuccessFalseForForbiddenFactoryCall()
    {
        var result = Result<object>.Forbidden();

        Assert.False(result.IsSuccess);
    }

    [Fact]
    public void InitializedIsSuccessFalseForInvalidListFactoryCall()
    {
        var result = Result<object>.Invalid(new List<ValidationError>());

        Assert.False(result.IsSuccess);
    }

    [Fact]
    public void InitializedIsSuccessFalseForInvalidFactoryCall()
    {
        var result = Result<object>.Invalid(new ValidationError());

        Assert.False(result.IsSuccess);
    }

    [Fact]
    public void InitializedIsSuccessFalseForNotFoundFactoryCall()
    {
        var result = Result<object>.NotFound();

        Assert.False(result.IsSuccess);
    }

    [Fact]
    public void InitializedIsSuccessFalseForConflictFactoryCall()
    {
        var result = Result<object>.Conflict();

        result.IsSuccess.Should().BeFalse();
    }

    [Fact]
    public void InitializedIsSuccessFalseForCriticalErrorFactoryCall()
    {
        var result = Result<object>.CriticalError();

        Assert.False(result.IsSuccess);
    }

    [Fact]
    public void InitializesStatusToNoContentForNoContentFactoryCall()
    {
        var result = Result<object>.NoContent();

        Assert.True(result.IsSuccess);
    }
}
