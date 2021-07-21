using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using Xunit;

namespace Ardalis.Result.UnitTests
{
    public class ResultConstructor
    {
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

        private class TestObject
        {
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

        [Fact]
        public void InitializesStatusToErrorGivenErrorFactoryCall()
        {
            var result = Result<object>.Error();

            Assert.Equal(ResultStatus.Error, result.Status);
        }

        [Fact]
        public void InitializesStatusToErrorAndSetsErrorMessageGivenErrorFactoryCall()
        {
            string errorMessage = "Something bad happened.";
            var result = Result<object>.Error(errorMessage);

            Assert.Equal(ResultStatus.Error, result.Status);
            Assert.Equal(errorMessage, result.Errors.First());
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
        }

        [Fact]
        public void InitializesStatusToForbiddenGivenForbiddenFactoryCall()
        {
            var result = Result<object>.Forbidden();

            Assert.Equal(ResultStatus.Forbidden, result.Status);
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
            var result = Result<object>.Error(null);

            Assert.False(result.IsSuccess);
        }

        [Fact]
        public void InitializedIsSuccessFalseForForbiddenFactoryCall()
        {
            var result = Result<object>.Forbidden();

            Assert.False(result.IsSuccess);
        }

        [Fact]
        public void InitializedIsSuccessFalseForInvalidFactoryCall()
        {
            var result = Result<object>.Invalid(null);

            Assert.False(result.IsSuccess);
        }

        [Fact]
        public void InitializedIsSuccessFalseForNotFoundFactoryCall()
        {
            var result = Result<object>.NotFound();

            Assert.False(result.IsSuccess);
        }
    }
}
