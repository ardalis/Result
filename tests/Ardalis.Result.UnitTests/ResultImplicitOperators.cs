using Xunit;

namespace Ardalis.Result.UnitTests
{
    public class ResultImplicitOperators
    {
        private string successMessage = "Success";
        private string expectedString = "test string";
        private int expectedInt = 123;
        private TestObject expectedObject = new TestObject();
        private TestObject expectedNullObject = null;

        [Fact]
        public void ConvertFromStringValue()
        {
            var result = DoBusinessOperationExample(expectedString);

            Assert.Equal(expectedString, result.Value);
            Assert.Equal(ResultStatus.Ok, result.Status);
        }
        [Fact]
        public void ConvertToStringValue()
        {
            var result = GetValueForResultExample(Result<string>.Success(expectedString));

            Assert.Equal(expectedString, result);
        }

        [Fact]
        public void SuccessWithSuccessMessage()
        {
            var result = Result<string>.Success(expectedString, successMessage);

            Assert.Equal(successMessage, result.SuccessMessage);
            Assert.Equal(expectedString, result.Value);
        }

        [Fact]
        public void ConvertFromIntValue()
        {
            var result = DoBusinessOperationExample(expectedInt);

            Assert.Equal(expectedInt, result.Value);
            Assert.Equal(ResultStatus.Ok, result.Status);
        }
        [Fact]
        public void ConvertToIntValue()
        {
            var result = GetValueForResultExample(Result<int>.Success(expectedInt));

            Assert.Equal(expectedInt, result);
        }

        [Fact]
        public void ConvertFromObjectValue()
        {
            var result = DoBusinessOperationExample(expectedObject);

            Assert.Equal(expectedObject, result.Value);
            Assert.Equal(ResultStatus.Ok, result.Status);
        }
        [Fact]
        public void ConvertToObjectValue()
        {
            var result = GetValueForResultExample(Result<TestObject>.Success(expectedObject));

            Assert.Equal(expectedObject, result);
        }

        public Result<T> DoBusinessOperationExample<T>(T testValue) => testValue;
        public T GetValueForResultExample<T>(Result<T> testResult) => testResult;

        private class TestObject {}
    }
}
