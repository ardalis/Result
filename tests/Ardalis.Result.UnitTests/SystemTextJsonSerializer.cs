using System.Text.Json;
using Xunit;

namespace Ardalis.Result.UnitTests
{
    public class SystemTextJsonSerializer
    {
        [Fact]
        public void ShouldSerializeResultOfValueType()
        {
            var result = Result.Success(5);
            string expected = "{\"Value\":5,\"Status\":0,\"IsSuccess\":true,\"SuccessMessage\":\"\",\"CorrelationId\":\"\",\"Errors\":[],\"ValidationErrors\":[]}";

            var json = JsonSerializer.Serialize(result);

            Assert.Equal(expected, json);
        }

        [Fact]
        public void ShouldSerializeResultOfReferenceType()
        {
            var result = Result.Success(new Foo { Bar = "Result!" });
            string expected = "{\"Value\":{\"Bar\":\"Result!\"},\"Status\":0,\"IsSuccess\":true,\"SuccessMessage\":\"\",\"CorrelationId\":\"\",\"Errors\":[],\"ValidationErrors\":[]}";

            var json = JsonSerializer.Serialize(result);

            Assert.Equal(expected, json);
        }

        [Fact]
        public void ShouldDeserializeResultOfValueType()
        {
            var expected = Result.Success(10);
            string json = "{\"Value\":10,\"Status\":0,\"IsSuccess\":true,\"SuccessMessage\":\"\",\"CorrelationId\":\"\",\"Errors\":[],\"ValidationErrors\":[]}";

            var result = JsonSerializer.Deserialize<Result<int>>(json);

            Assert.Equivalent(expected, result);
        }

        [Fact]
        public void ShouldDeserializeResultOfReferenceType()
        {
            var expected = Result.Success(new Foo { Bar = "Result!" });
            string json = "{\"Value\":{\"Bar\":\"Result!\"},\"Status\":0,\"IsSuccess\":true,\"SuccessMessage\":\"\",\"CorrelationId\":\"\",\"Errors\":[],\"ValidationErrors\":[]}";

            var result = JsonSerializer.Deserialize<Result<Foo>>(json);

            Assert.Equivalent(expected, result);
        }

        private class Foo
        {
            public string Bar { get; set; }
        }
    }
}
