using System;
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


    }
}
