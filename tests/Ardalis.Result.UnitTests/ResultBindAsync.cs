using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Ardalis.Result.UnitTests
{
    public class ResultBindAsync
    {
        [Fact]
        public async Task BindAsync_WithSuccessResultAndAsyncFunction_ReturnsSuccess()
        {
            int successValue = 123;
            var result = Result<int>.Success(successValue);
            var expected = Result<string>.Success(successValue.ToString());

            var actual = await result.BindAsync(async val =>
            {
                await Task.Delay(1);
                return Result<string>.Success(val.ToString());
            });

            actual.Should().BeEquivalentTo(expected);
        }

        [Fact]
        public async Task BindAsync_WithAsyncResultAndAsyncFunction_ReturnsSuccess()
        {
            int successValue = 123;
            var resultTask = Task.FromResult(Result<int>.Success(successValue));
            var expected = Result<string>.Success(successValue.ToString());

            var actual = await resultTask.BindAsync(async val =>
            {
                await Task.Delay(1);
                return Result<string>.Success(val.ToString());
            });

            actual.Should().BeEquivalentTo(expected);
        }

        [Fact]
        public async Task BindAsync_VoidAsyncResultToAsyncResult_ReturnsSuccess()
        {
            int successValue = 123;
            var resultTask = Task.FromResult(Result.Success());
            var expected = Result<int>.Success(successValue);

            var actual = await resultTask.BindAsync(async _ =>
            {
                await Task.Delay(1);
                return Result<int>.Success(successValue);
            });

            actual.Should().BeEquivalentTo(expected);
        }

        [Fact]
        public async Task BindAsync_VoidResultToAsyncResult_ReturnsSuccess()
        {
            int successValue = 123;
            var result = Result.Success();
            var expected = Result<int>.Success(successValue);

            var actual = await result.BindAsync(async _ =>
            {
                await Task.Delay(1);
                return Result<int>.Success(successValue);
            });

            actual.Should().BeEquivalentTo(expected);
        }

        [Fact]
        public async Task BindAsync_WithAsyncResultAndSyncFunction_ReturnsSuccess()
        {
            int successValue = 123;
            var resultTask = Task.FromResult(Result<int>.Success(successValue));
            var expected = Result<string>.Success(successValue.ToString());

            var actual = await resultTask.BindAsync(val => Result<string>.Success(val.ToString()));

            actual.Should().BeEquivalentTo(expected);
        }

        [Fact]
        public async Task BindAsync_WithComplexTypeAndAsyncFunction_ReturnsSuccess()
        {
            var foo = new Foo("Bar");
            var result = Result<Foo>.Success(foo);
            var expected = Result<FooDto>.Success(new FooDto(foo.Bar));

            var actual = await result.BindAsync(async fooValue =>
            {
                await Task.Delay(1);
                return Result<FooDto>.Success(new FooDto(fooValue.Bar));
            });

            actual.Should().BeEquivalentTo(expected);
        }

        [Fact]
        public async Task BindAsync_WithCreatedResultAndAsyncFunction_ReturnsCreated()
        {
            int createdValue = 123;
            var result = Result<int>.Created(createdValue);
            var expected = Result<string>.Created(createdValue.ToString());

            var actual = await result.BindAsync(async val =>
            {
                await Task.Delay(1);
                return Result<string>.Created(val.ToString());
            });

            actual.Status.Should().Be(ResultStatus.Created);
            actual.Value.Should().Be(expected.Value);
        }

        [Fact]
        public async Task BindAsync_WithNotFoundResult_PropagatesNotFound()
        {
            var result = Result<int>.NotFound();

            var actual = await result.BindAsync(async val =>
            {
                await Task.Delay(1);
                return Result<string>.Success(val.ToString());
            });

            actual.Status.Should().Be(ResultStatus.NotFound);
            actual.Value.Should().BeNull();
        }

        [Fact]
        public async Task BindAsync_WithErrorInBindFunction_PropagatesError()
        {
            int successValue = 123;
            var result = Result<int>.Success(successValue);
            string expectedError = "Bind function failed";

            var actual = await result.BindAsync(async val =>
            {
                await Task.Delay(1);
                return Result<string>.Error(expectedError);
            });

            actual.Status.Should().Be(ResultStatus.Error);
            actual.Errors.Single().Should().Be(expectedError);
            actual.Value.Should().BeNull();
        }

        [Fact]
        public async Task BindAsync_WithValidationErrorsInBindFunction_ReturnsInvalid()
        {
            int successValue = 1;
            var result = Result<int>.Success(successValue);
            var validationErrors = new List<ValidationError>
        {
            new() { ErrorMessage = "Value must be greater than 1" }
        };

            var actual = await result.BindAsync(async val =>
            {
                await Task.Delay(1);
                if (val > 1)
                {
                    return Result<string>.Success("Valid value");
                }
                else
                {
                    return Result<string>.Invalid(validationErrors);
                }
            });

            actual.Status.Should().Be(ResultStatus.Invalid);
            actual.ValidationErrors.Should().BeEquivalentTo(validationErrors);
            actual.Value.Should().BeNull();
        }

        [Fact]
        public async Task BindAsync_WithUnauthorizedResult_PropagatesUnauthorized()
        {
            var result = Result<int>.Unauthorized();

            var actual = await result.BindAsync(async val =>
            {
                await Task.Delay(1);
                return Result<string>.Success(val.ToString());
            });

            actual.Status.Should().Be(ResultStatus.Unauthorized);
        }

        [Fact]
        public async Task BindAsync_WithForbiddenResult_PropagatesForbidden()
        {
            var result = Result<int>.Forbidden();

            var actual = await result.BindAsync(async val =>
            {
                await Task.Delay(1);
                return Result<string>.Success(val.ToString());
            });

            actual.Status.Should().Be(ResultStatus.Forbidden);
        }

        [Fact]
        public async Task BindAsync_AsyncResultWithErrorInBindFunction_ReturnsError()
        {
            var resultTask = Task.FromResult(Result<int>.Success(123));

            var actual = await resultTask.BindAsync(async val =>
            {
                await Task.Delay(1);
                return Result<string>.Error("Async error");
            });

            actual.Status.Should().Be(ResultStatus.Error);
            actual.Errors.Single().Should().Be("Async error");
        }

        [Fact]
        public async Task BindAsync_AsyncResultWithSyncBindFunction_ReturnsSuccess()
        {
            var resultTask = Task.FromResult(Result<int>.Success(123));
            var expected = Result<string>.Success("123");

            var actual = await resultTask.BindAsync(val => Result<string>.Success(val.ToString()));

            actual.Should().BeEquivalentTo(expected);
        }

        [Fact]
        public async Task BindAsync_VoidAsyncResultToVoidAsyncResult_ReturnsSuccess()
        {
            Task<Result> resultTask = Task.FromResult(Result.Success());
            Result expected = Result.Success();

            var actual = await resultTask.BindAsync(async r =>
            {
                await Task.Delay(1);
                return Result.Success();
            });

            actual.Should().BeEquivalentTo(expected);
        }

        [Fact]
        public async Task BindAsync_VoidResultToAsyncResultWithValue_ReturnsSuccessWithValue()
        {
            int expectedValue = 123;
            var result = Result.Success();

            var actual = await result.BindAsync(async _ =>
            {
                await Task.Delay(1);
                return Result<int>.Success(expectedValue);
            });

            actual.Status.Should().Be(ResultStatus.Ok);
            actual.Value.Should().Be(expectedValue);
        }

        [Fact]
        public async Task BindAsync_AsyncResultWithValueToVoidAsyncResult_ReturnsSuccess()
        {
            var resultTask = Task.FromResult(Result<int>.Success(123));

            var actual = await resultTask.BindAsync(async val =>
            {
                await Task.Delay(1);
                return Result.Success();
            });

            actual.Status.Should().Be(ResultStatus.Ok);
        }

        [Fact]
        public async Task BindAsync_AsyncResultWithValueToVoidResult_ReturnsSuccess()
        {
            var resultTask = Task.FromResult(Result<int>.Success(123));

            var actual = await resultTask.BindAsync(val => Result.Success());

            actual.Status.Should().Be(ResultStatus.Ok);
        }

        [Fact]
        public async Task BindAsync_VoidAsyncResultWithoutReturnValue_ReturnsSuccess()
        {
            var resultTask = Task.FromResult(Result.Success());

            var actual = await resultTask.BindAsync(async _ =>
            {
                await Task.Delay(1);
                return Result.Success();
            });

            actual.Status.Should().Be(ResultStatus.Ok);
        }

        [Fact]
        public async Task BindAsync_WithNoContentResult_PropagatesNoContent()
        {
            var result = Result<int>.NoContent();

            var actual = await result.BindAsync(async val =>
            {
                await Task.Delay(1);
                return Result<string>.Success(val.ToString());
            });

            actual.Status.Should().Be(ResultStatus.NoContent);
            actual.Value.Should().BeNull();
            actual.Errors.Should().BeEmpty();
            actual.ValidationErrors.Should().BeEmpty();
        }

        [Fact]
        public async Task BindAsync_DoesNotInvokeBindFunctionWhenResultIsError()
        {
            var result = Result<int>.Error("Initial error");

            bool bindFunctionInvoked = false;

            var actual = await result.BindAsync(async val =>
            {
                bindFunctionInvoked = true;
                await Task.Delay(1);
                return Result<string>.Success(val.ToString());
            });

            bindFunctionInvoked.Should().BeFalse();
            actual.Status.Should().Be(ResultStatus.Error);
            actual.Errors.Single().Should().Be("Initial error");
        }

        [Fact]
        public async Task BindAsync_AsyncResultWithValueToVoidResultUsingSyncBindFunction_ReturnsSuccess()
        {
            int inputValue = 123;
            var resultTask = Task.FromResult(Result<int>.Success(inputValue));
            var expected = Result.Success();

            var actual = await resultTask.BindAsync(val => Result.Success());

            actual.Should().BeEquivalentTo(expected);
        }

        [Fact]
        public async Task BindAsync_WithAsyncResultError_DoesNotInvokeBindFunction()
        {
            string errorMessage = "Initial error";
            var resultTask = Task.FromResult(Result<int>.Error(errorMessage));

            var actual = await resultTask.BindAsync(val => Result.Success());

            actual.Status.Should().Be(ResultStatus.Error);
            actual.Errors.Single().Should().Be(errorMessage);
        }

        [Fact]
        public async Task BindAsync_SyncBindFunctionReturnsError_ReturnsError()
        {
            int inputValue = 123;
            string bindErrorMessage = "Error in bind function";
            var resultTask = Task.FromResult(Result<int>.Success(inputValue));

            var actual = await resultTask.BindAsync(val => Result.Error(bindErrorMessage));

            actual.Status.Should().Be(ResultStatus.Error);
            actual.Errors.Single().Should().Be(bindErrorMessage);
        }

        [Fact]
        public async Task BindAsync_SyncResultWithAsyncBindFunction_ReturnsSuccess()
        {
            int inputValue = 123;
            var result = Result<int>.Success(inputValue);
            var expected = Result.Success();

            var actual = await result.BindAsync(async val =>
            {
                await Task.Delay(1);
                return Result.Success();
            });

            actual.Should().BeEquivalentTo(expected);
        }

        [Fact]
        public async Task BindAsync_WithSyncResultError_DoesNotInvokeBindFunction()
        {
            string errorMessage = "Initial error";
            var result = Result<int>.Error(errorMessage);

            var actual = await result.BindAsync(async val =>
            {
                await Task.Delay(1);
                return Result.Success();
            });

            actual.Status.Should().Be(ResultStatus.Error);
            actual.Errors.Single().Should().Be(errorMessage);
        }

        [Fact]
        public async Task BindAsync_AsyncBindFunctionReturnsError_ReturnsError()
        {
            int inputValue = 123;
            string bindErrorMessage = "Error in bind function";
            var result = Result<int>.Success(inputValue);

            var actual = await result.BindAsync(async val =>
            {
                await Task.Delay(1);
                return Result.Error(bindErrorMessage);
            });

            actual.Status.Should().Be(ResultStatus.Error);
            actual.Errors.Single().Should().Be(bindErrorMessage);
        }

        [Fact]
        public async Task BindAsync_VoidResultWithAsyncBindFunction_ReturnsSuccess()
        {
            var result = Result.Success();
            var expected = Result.Success();

            var actual = await result.BindAsync(async res =>
            {
                await Task.Delay(1);
                return Result.Success();
            });

            actual.Should().BeEquivalentTo(expected);
        }

        [Fact]
        public async Task BindAsync_WithVoidResultError_DoesNotInvokeBindFunction()
        {
            string errorMessage = "Initial error";
            var result = Result.Error(errorMessage);

            var actual = await result.BindAsync(async res =>
            {
                await Task.Delay(1);
                return Result.Success();
            });

            actual.Status.Should().Be(ResultStatus.Error);
            actual.Errors.Single().Should().Be(errorMessage);
        }

        [Fact]
        public async Task BindAsync_AsyncBindFunctionReturnsErrorWithVoidResult_ReturnsError()
        {
            var result = Result.Success();
            string bindErrorMessage = "Error in bind function";

            var actual = await result.BindAsync(async res =>
            {
                await Task.Delay(1);
                return Result.Error(bindErrorMessage);
            });

            actual.Status.Should().Be(ResultStatus.Error);
            actual.Errors.Single().Should().Be(bindErrorMessage);
        }

        [Fact]
        public async Task BindAsync_AsyncVoidResultWithAsyncBindFunction_ReturnsSuccess()
        {
            var resultTask = Task.FromResult(Result.Success());
            var expected = Result.Success();

            var actual = await resultTask.BindAsync(async res =>
            {
                await Task.Delay(1);
                return Result.Success();
            });

            actual.Should().BeEquivalentTo(expected);
        }

        [Fact]
        public async Task BindAsync_WithAsyncVoidResultError_DoesNotInvokeBindFunction()
        {
            string errorMessage = "Initial error";
            var resultTask = Task.FromResult(Result.Error(errorMessage));

            var actual = await resultTask.BindAsync(async res =>
            {
                await Task.Delay(1);
                return Result.Success();
            });

            actual.Status.Should().Be(ResultStatus.Error);
            actual.Errors.Single().Should().Be(errorMessage);
        }

        [Fact]
        public async Task BindAsync_AsyncBindFunctionReturnsErrorWithAsyncVoidResult_ReturnsError()
        {
            var resultTask = Task.FromResult(Result.Success());
            string bindErrorMessage = "Error in bind function";

            var actual = await resultTask.BindAsync(async res =>
            {
                await Task.Delay(1);
                return Result.Error(bindErrorMessage);
            });

            actual.Status.Should().Be(ResultStatus.Error);
            actual.Errors.Single().Should().Be(bindErrorMessage);
        }

        [Fact]
        public async Task BindAsync_VoidAsyncResultWithAsyncBindFunction_ReturnsSuccess()
        {
            var resultTask = Task.FromResult(Result.Success());
            var expected = Result.Success();

            var actual = await resultTask.BindAsync(async res =>
            {
                await Task.Delay(1);
                return Result.Success();
            });

            actual.Should().BeEquivalentTo(expected);
        }

        [Fact]
        public async Task BindAsync_WithVoidAsyncResultError_DoesNotInvokeBindFunction()
        {
            var errorMessage = "Initial error";
            var resultTask = Task.FromResult(Result.Error(errorMessage));

            var actual = await resultTask.BindAsync(async res =>
            {
                await Task.Delay(1);
                return Result.Success();
            });

            actual.Status.Should().Be(ResultStatus.Error);
            actual.Errors.Single().Should().Be(errorMessage);
        }

        [Fact]
        public async Task BindAsync_SyncResultWithValueAndAsyncBindFunction_ReturnsSuccess()
        {
            int inputValue = 123;
            var result = Result<int>.Success(inputValue);
            var expected = Result.Success();

            var actual = await result.BindAsync(async val =>
            {
                await Task.Delay(1);
                return Result.Success();
            });

            actual.Should().BeEquivalentTo(expected);
        }

        [Fact]
        public async Task BindAsync_TaskResultTSourceWithSyncBindFunction_ReturnsSuccess()
        {
            // Arrange
            Task<Result<int>> resultTask = Task.FromResult(Result<int>.Success(123));
            var expected = Result.Success();

            // Act
            var actual = await resultTask.BindAsync(val => Result.Success());

            // Assert
            actual.Should().BeEquivalentTo(expected);
        }

        [Fact]
        public async Task BindAsync_VoidResultToTaskVoidResult_ReturnsSuccess()
        {
            // Arrange
            var result = Result<int>.Success(123);
            var expected = Result.Success();

            // Act
            var actual = await result.BindAsync(async val =>
            {
                await Task.Delay(1);
                return Result.Success();
            });

            // Assert
            actual.Should().BeEquivalentTo(expected);
        }

        [Fact]
        public async Task BindAsync_WithSyncResultWithValueError_DoesNotInvokeBindFunction()
        {
            var errorMessage = "Initial error";
            var result = Result<int>.Error(errorMessage);

            var actual = await result.BindAsync(async val =>
            {
                await Task.Delay(1);
                return Result.Success();
            });

            actual.Status.Should().Be(ResultStatus.Error);
            actual.Errors.Single().Should().Be(errorMessage);
        }

        [Fact]
        public async Task BindAsync_AsyncBindFunctionReturnsErrorOnResultWithValue_ReturnsError()
        {
            int inputValue = 123;
            var result = Result<int>.Success(inputValue);
            var bindErrorMessage = "Error in bind function";

            var actual = await result.BindAsync(async val =>
            {
                await Task.Delay(1);
                return Result.Error(bindErrorMessage);
            });

            actual.Status.Should().Be(ResultStatus.Error);
            actual.Errors.Single().Should().Be(bindErrorMessage);
        }

        [Fact]
        public async Task BindAsync_AsyncResultWithValueAndSyncBindFunction_ReturnsSuccess()
        {
            int inputValue = 123;
            var resultTask = Task.FromResult(Result<int>.Success(inputValue));
            var expected = Result.Success();

            var actual = await resultTask.BindAsync(val => Result.Success());

            actual.Should().BeEquivalentTo(expected);
        }

        [Fact]
        public async Task BindAsync_WithAsyncResultWithValueError_DoesNotInvokeBindFunction()
        {
            var errorMessage = "Initial error";
            var resultTask = Task.FromResult(Result<int>.Error(errorMessage));

            var actual = await resultTask.BindAsync(val => Result.Success());

            actual.Status.Should().Be(ResultStatus.Error);
            actual.Errors.Single().Should().Be(errorMessage);
        }

        [Fact]
        public async Task BindAsync_SyncBindFunctionReturnsErrorOnAsyncResultWithValue_ReturnsError()
        {
            int inputValue = 123;
            var resultTask = Task.FromResult(Result<int>.Success(inputValue));
            var bindErrorMessage = "Error in bind function";

            var actual = await resultTask.BindAsync(val => Result.Error(bindErrorMessage));

            actual.Status.Should().Be(ResultStatus.Error);
            actual.Errors.Single().Should().Be(bindErrorMessage);
        }


        [Fact]
        public async Task BindAsync_TaskResultTSourceWithSyncBindFunction_ReturnsError()
        {
            // Arrange
            int successValue = 123;
            var resultTask = Task.FromResult(Result<int>.Success(successValue));
            string bindErrorMessage = "Error in bind function";

            // Act
            var actual = await resultTask.BindAsync(val => Result.Error(bindErrorMessage));

            // Assert
            actual.Status.Should().Be(ResultStatus.Error);
            actual.Errors.Single().Should().Be(bindErrorMessage);
        }

        [Fact]
        public async Task BindAsync_TaskResultTSourceError_DoesNotInvokeBindFunction()
        {
            // Arrange
            string errorMessage = "Initial error";
            var resultTask = Task.FromResult(Result<int>.Error(errorMessage));
            bool bindFunctionInvoked = false;

            // Act
            var actual = await resultTask.BindAsync(val =>
            {
                bindFunctionInvoked = true;
                return Result.Success();
            });

            // Assert
            bindFunctionInvoked.Should().BeFalse();
            actual.Status.Should().Be(ResultStatus.Error);
            actual.Errors.Single().Should().Be(errorMessage);
        }

        [Fact]
        public async Task BindAsync_TaskResultTSourceCreatedWithSyncBindFunction_ReturnsResult()
        {
            // Arrange
            int createdValue = 123;
            var resultTask = Task.FromResult(Result<int>.Created(createdValue));

            // Act
            var actual = await resultTask.BindAsync(val => Result.Success());

            // Assert
            actual.Status.Should().Be(ResultStatus.Ok);
        }

        [Fact]
        public async Task BindAsync_ResultTSourceWithAsyncBindFunction_ReturnsSuccess()
        {
            // Arrange
            int successValue = 123;
            var result = Result<int>.Success(successValue);
            var expected = Result.Success();

            // Act
            var actual = await result.BindAsync(async val =>
            {
                await Task.Delay(1);
                return Result.Success();
            });

            // Assert
            actual.Should().BeEquivalentTo(expected);
        }

        [Fact]
        public async Task BindAsync_ResultTSourceWithAsyncBindFunction_ReturnsError()
        {
            // Arrange
            int successValue = 123;
            var result = Result<int>.Success(successValue);
            string bindErrorMessage = "Error in bind function";

            // Act
            var actual = await result.BindAsync(async val =>
            {
                await Task.Delay(1);
                return Result.Error(bindErrorMessage);
            });

            // Assert
            actual.Status.Should().Be(ResultStatus.Error);
            actual.Errors.Single().Should().Be(bindErrorMessage);
        }

        [Fact]
        public async Task BindAsync_ResultTSourceError_DoesNotInvokeBindFunction()
        {
            // Arrange
            string errorMessage = "Initial error";
            var result = Result<int>.Error(errorMessage);
            bool bindFunctionInvoked = false;

            // Act
            var actual = await result.BindAsync(async val =>
            {
                bindFunctionInvoked = true;
                await Task.Delay(1);
                return Result.Success();
            });

            // Assert
            bindFunctionInvoked.Should().BeFalse();
            actual.Status.Should().Be(ResultStatus.Error);
            actual.Errors.Single().Should().Be(errorMessage);
        }

        private record Foo(string Bar);

        private class FooDto
        {
            public string Bar { get; set; }

            public FooDto(string bar)
            {
                Bar = bar;
            }

            public static async Task<Result<FooDto>> CreateFromFooResultAsync(Foo foo)
            {
                await Task.Delay(1);
                return Result<FooDto>.Success(new FooDto(foo.Bar));
            }
        }
    }

}
