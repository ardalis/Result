using System;
using System.Collections.Generic;
using System.Linq;
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
            List<string> validationErrors = new List<string>
            {
                "Name is required",
                "Name cannot exceed 10 characters"
            };
            var result = Result<object>.Invalid(validationErrors.ToArray());

            Assert.Equal(ResultStatus.Invalid, result.Status);
            Assert.Equal("Name is required", result.Errors.First());
            Assert.Equal("Name cannot exceed 10 characters", result.Errors.Last());
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


    }
}
