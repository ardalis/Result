using FluentAssertions;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Ardalis.Result.UnitTests
{
    public class ResultMapAsync
    {
        [Fact]
        public async Task ShouldProduceReturnValueFromSuccessAsyncFunction()
        {
            int successValue = 123;
            var result = Result<int>.Success(successValue);
            string expected = successValue.ToString();

            var actual = await result.MapAsync(async val =>
            {
                await Task.Delay(1); 
                return val.ToString();
            });

            expected.Should().BeEquivalentTo(actual.Value);
            actual.Status.Should().Be(ResultStatus.Ok);
        }

        [Fact]
        public async Task ShouldProduceReturnValueFromAsyncResultAndAsyncFunction()
        {
            int successValue = 123;
            var resultTask = Task.FromResult(Result<int>.Success(successValue));
            string expected = successValue.ToString();

            var actual = await resultTask.MapAsync(async val =>
            {
                await Task.Delay(1); 
                return val.ToString();
            });

            expected.Should().BeEquivalentTo(actual.Value);
            actual.Status.Should().Be(ResultStatus.Ok);
        }

        [Fact]
        public async Task ShouldProduceComplexTypeReturnValueFromSuccessAsyncFunction()
        {
            var foo = new Foo("Bar");
            var result = Result<Foo>.Success(foo);

            var actual = await result.MapAsync(async fooValue =>
            {
                await Task.Delay(1); 
                return new FooDto(fooValue.Bar);
            });

            actual.Value.Bar.Should().Be(foo.Bar);
            actual.Status.Should().Be(ResultStatus.Ok);
        }

        [Fact]
        public async Task ShouldProduceReturnValueFromCreatedAsyncFunction()
        {
            int createdValue = 123;
            var result = Result<int>.Created(createdValue);
            string expected = createdValue.ToString();

            var actual = await result.MapAsync(async val =>
            {
                await Task.Delay(1); 
                return val.ToString();
            });

            expected.Should().BeEquivalentTo(actual.Value);
            actual.Status.Should().Be(ResultStatus.Created);
        }

        [Fact]
        public async Task ShouldProduceNotFoundAsync()
        {
            var result = Result<int>.NotFound();

            var actual = await result.MapAsync(async val =>
            {
                await Task.Delay(1); 
                return val.ToString();
            });

            actual.Status.Should().Be(ResultStatus.NotFound);
            actual.Value.Should().BeNull();
        }

        [Fact]
        public async Task ShouldProduceErrorResultWithErrorsAsync()
        {
            var errorList = new ErrorList(["Error 1", "Error 2"], default);
            var result = Result<int>.Error(errorList);

            var actual = await result.MapAsync(async val =>
            {
                await Task.Delay(1); 
                return val.ToString();
            });

            actual.Status.Should().Be(ResultStatus.Error);
            actual.Errors.Should().BeEquivalentTo(errorList.ErrorMessages);
            actual.Value.Should().BeNull();
        }

        [Fact]
        public async Task ShouldHandleExceptionInMapFunctionAsync()
        {
            int successValue = 123;
            var result = Result<int>.Success(successValue);

            Func<Task> action = async () =>
            {
                await result.MapAsync<int, string>(async val =>
                {
                    await Task.Delay(1); 
                    throw new InvalidOperationException("Test exception");
                });
            };

            await action.Should().ThrowAsync<InvalidOperationException>().WithMessage("Test exception");
        }

        [Fact]
        public async Task ShouldNotInvokeMapFunctionWhenResultIsErrorAsync()
        {
            var result = Result<int>.Error("Initial error");

            bool mapFunctionInvoked = false;

            var actual = await result.MapAsync(async val =>
            {
                mapFunctionInvoked = true;
                await Task.Delay(1); 
                return val.ToString();
            });

            mapFunctionInvoked.Should().BeFalse();
            actual.Status.Should().Be(ResultStatus.Error);
            actual.Errors.Single().Should().Be("Initial error");
            actual.Value.Should().BeNull();
        }

        [Fact]
        public async Task ShouldPreserveLocationInCreatedStatusAsync()
        {
            var foo = new Foo("Bar");
            var result = Result<Foo>.Created(foo, location: "/foo/1");

            var actual = await result.MapAsync(async val =>
            {
                await Task.Delay(1); 
                return new FooDto(val.Bar);
            });

            actual.Status.Should().Be(ResultStatus.Created);
            actual.Value.Bar.Should().Be(foo.Bar);
            actual.Location.Should().Be("/foo/1");
        }

        [Fact]
        public async Task ShouldHandleAsyncResultAndAsyncMapFunctionWithErrorStatus()
        {
            var resultTask = Task.FromResult(Result<int>.NotFound("Item not found"));

            var actual = await resultTask.MapAsync(async val =>
            {
                await Task.Delay(1); 
                return val.ToString();
            });

            actual.Status.Should().Be(ResultStatus.NotFound);
            actual.Errors.Single().Should().Be("Item not found");
            actual.Value.Should().BeNull();
        }

        [Fact]
        public async Task ShouldProduceNoContentAsync()
        {
            var result = Result<int>.NoContent();

            var actual = await result.MapAsync(async val =>
            {
                await Task.Delay(1); 
                return val.ToString();
            });

            actual.Status.Should().Be(ResultStatus.NoContent);
            actual.Value.Should().BeNull();
            actual.Errors.Should().BeEmpty();
            actual.ValidationErrors.Should().BeEmpty();
        }

        private record Foo(string Bar);

        private class FooDto
        {
            public string Bar { get; set; }

            public FooDto(string bar)
            {
                Bar = bar;
            }
        }
    }
}
