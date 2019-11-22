using System;
using Xunit;

namespace Ardalis.Result.UnitTests
{
    public class ResultConstructor
    {
        [Fact]
        public void InitializesStronglyTypedValue()
        {
            var result = new Result<string>("test string");

            Assert.Equal("test string", result.Value);
        }
    }
}
