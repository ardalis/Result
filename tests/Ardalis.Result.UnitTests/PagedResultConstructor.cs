using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace Ardalis.Result.UnitTests;

public class PagedResultConstructor
{
    private readonly PagedInfo _pagedInfo = new PagedInfo(0, 10, 1, 3);

    [Fact]
    public void InitializesStronglyTypedStringValue()
    {
        string expectedString = "test string";
        var result = new PagedResult<string>(_pagedInfo, expectedString);

        Assert.Equal(expectedString, result.Value);
        Assert.Equal(_pagedInfo, result.PagedInfo);
    }

    [Fact]
    public void InitializesStronglyTypedIntValue()
    {
        int expectedInt = 123;
        var result = new PagedResult<int>(_pagedInfo, expectedInt);

        Assert.Equal(expectedInt, result.Value);
        Assert.Equal(_pagedInfo, result.PagedInfo);
    }

    [Fact]
    public void InitializesStronglyTypedObjectValue()
    {
        var expectedObject = new TestObject();
        var result = new PagedResult<TestObject>(_pagedInfo, expectedObject);

        Assert.Equal(expectedObject, result.Value);
        Assert.Equal(_pagedInfo, result.PagedInfo);
    }

    private class TestObject
    {
    }

    [Fact]
    public void InitializesValueToNullGivenNullConstructorArgument()
    {
        var result = new PagedResult<object>(_pagedInfo, null);

        Assert.Null(result.Value);
        Assert.Equal(_pagedInfo, result.PagedInfo);
    }

    [Theory]
    [InlineData(null)]
    [InlineData(123)]
    [InlineData("test value")]
    public void InitializesStatusToOkGivenValue(object value)
    {
        var result = new PagedResult<object>(_pagedInfo, value);

        Assert.Equal(ResultStatus.Ok, result.Status);
        Assert.Equal(_pagedInfo, result.PagedInfo);
    }

    [Theory]
    [InlineData(null)]
    [InlineData(123)]
    [InlineData("test value")]
    public void InitializesValueUsingFactoryMethodAndSetsStatusToOk(object value)
    {
        var result = Result<object>
            .Success(value)
            .ToPagedResult(_pagedInfo);

        Assert.Equal(ResultStatus.Ok, result.Status);
        Assert.Equal(value, result.Value);
        Assert.Equal(_pagedInfo, result.PagedInfo);
    }

    [Fact]
    public void InitializesStatusToErrorGivenErrorFactoryCall()
    {
        var result = Result<object>
            .Error()
            .ToPagedResult(_pagedInfo);

        Assert.Equal(ResultStatus.Error, result.Status);
        Assert.Equal(_pagedInfo, result.PagedInfo);
    }

    [Fact]
    public void InitializesStatusToErrorAndSetsErrorMessageGivenErrorFactoryCall()
    {
        string errorMessage = "Something bad happened.";
        string correlationId = Guid.NewGuid().ToString();
        ErrorList errors = new(new[] { errorMessage }, correlationId);

        var result = Result<object>
            .Error(errors)
            .ToPagedResult(_pagedInfo);

        Assert.Equal(ResultStatus.Error, result.Status);
        Assert.Equal(errorMessage, result.Errors.Single());
        Assert.Equal(correlationId, result.CorrelationId);
        Assert.Equal(_pagedInfo, result.PagedInfo);
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
        var result = Result<object>
            .Invalid(validationErrors)
            .ToPagedResult(_pagedInfo);

        result.Status.Should().Be(ResultStatus.Invalid);
        result.ValidationErrors.Should().ContainEquivalentOf(new ValidationError { ErrorMessage = "Name is required", Identifier = "name" });
        result.ValidationErrors.Should().ContainEquivalentOf(new ValidationError { ErrorMessage = "PostalCode cannot exceed 10 characters", Identifier = "postalCode" });
        result.PagedInfo.Should().Be(_pagedInfo);
    }

    [Fact]
    public void InitializesStatusToNotFoundGivenNotFoundFactoryCall()
    {
        var result = Result<object>
            .NotFound()
            .ToPagedResult(_pagedInfo);

        Assert.Equal(ResultStatus.NotFound, result.Status);
        Assert.Equal(_pagedInfo, result.PagedInfo);
    }

    [Fact]
    public void InitializesStatusToForbiddenGivenForbiddenFactoryCall()
    {
        var result = Result<object>
            .Forbidden()
            .ToPagedResult(_pagedInfo);

        Assert.Equal(ResultStatus.Forbidden, result.Status);
        Assert.Equal(_pagedInfo, result.PagedInfo);
    }

    [Fact]
    public void InitializesStatusToUnavailableGivenUnavailableFactoryCall()
    {
        var result = Result<object>
            .Unavailable()
            .ToPagedResult(_pagedInfo);

        Assert.Equal(ResultStatus.Unavailable, result.Status);
        Assert.Equal(_pagedInfo, result.PagedInfo);
    }
}
