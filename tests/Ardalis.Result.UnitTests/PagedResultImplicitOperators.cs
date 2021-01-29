using System;
using Xunit;

namespace Ardalis.Result.UnitTests
{
    public class PagedResultImplicitOperators
    {
        private readonly PagedInfo _pagedInfo = new PagedInfo(0, 10, 1, 3);
        private string successMessage = "Success";
        private string expectedString = "test string";
        private int expectedInt = 123;
        private TestObject expectedObject = new TestObject();
        private TestObject expectedNullObject = null;

        [Fact]
        public void ConvertFromStringValue()
        {
            var result = DoBusinessOperationExample(expectedString)
                .ToPagedResult(_pagedInfo);

            Assert.Equal(expectedString, result.Value);
            Assert.Equal(ResultStatus.Ok, result.Status);
            Assert.Equal(_pagedInfo, result.PagedInfo);
        }
        [Fact]
        public void ConvertToStringValue()
        {
            var result = GetValueForPagedResultExample(Result<string>
                .Success(expectedString)
                .ToPagedResult(_pagedInfo));

            Assert.Equal(expectedString, result);
        }

        [Fact]
        public void SuccessWithSuccessMessage()
        {
            var result = Result<string>
                .Success(expectedString, successMessage)
                .ToPagedResult(_pagedInfo);

            Assert.Equal(successMessage, result.SuccessMessage);
            Assert.Equal(expectedString, result.Value);
            Assert.Equal(_pagedInfo, result.PagedInfo);
        }

        [Fact]
        public void ConvertFromIntValue()
        {
            var result = DoBusinessOperationExample(expectedInt).ToPagedResult(_pagedInfo);

            Assert.Equal(expectedInt, result.Value);
            Assert.Equal(ResultStatus.Ok, result.Status);
            Assert.Equal(_pagedInfo, result.PagedInfo);
        }
        [Fact]
        public void ConvertToIntValue()
        {
            var result = GetValueForPagedResultExample(Result<int>.Success(expectedInt).ToPagedResult(_pagedInfo));

            Assert.Equal(expectedInt, result);
        }

        [Fact]
        public void ConvertFromObjectValue()
        {
            var result = DoBusinessOperationExample(expectedObject).ToPagedResult(_pagedInfo);

            Assert.Equal(expectedObject, result.Value);
            Assert.Equal(ResultStatus.Ok, result.Status);
            Assert.Equal(_pagedInfo, result.PagedInfo);
        }
        [Fact]
        public void ConvertToObjectValue()
        {
            var result = GetValueForPagedResultExample(Result<TestObject>
                .Success(expectedObject)
                .ToPagedResult(_pagedInfo));

            Assert.Equal(expectedObject, result);
        }

        [Fact]
        public void ConvertFromNullObjectValueThrows()
        {
            Assert.Throws<ArgumentNullException>(() => DoBusinessOperationExample(expectedNullObject).ToPagedResult(_pagedInfo));
        }
        [Fact]
        public void ConvertToNullObjectValueThrows()
        {
            Assert.Throws<ArgumentNullException>(() => GetValueForPagedResultExample(Result<TestObject>.Success(expectedNullObject).ToPagedResult(_pagedInfo)));
        }

        public Result<T> DoBusinessOperationExample<T>(T testValue) => testValue;
        public T GetValueForPagedResultExample<T>(PagedResult<T> testResult) => testResult;

        private class TestObject {}
    }
}
