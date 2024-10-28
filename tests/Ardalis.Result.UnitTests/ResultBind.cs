using FluentAssertions;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace Ardalis.Result.UnitTests
{
    public class ResultBind
    {
        [Fact]
        public void ShouldProduceSuccessResultFromSuccess()
        {
            int successValue = 123;
            var result = Result<int>.Success(successValue);
            var expected = Result<string>.Success(successValue.ToString());

            var actual = result.Bind(val => Result<string>.Success(val.ToString()));

            actual.Should().BeEquivalentTo(expected);
        }

        [Fact]
        public void CanChainSeveralMethods()
        {
            var result = Result<int>.Success(123);
            var expected = Result<string>.Success("125");

            var actual = result.Bind(v => Result<int>.Success(v + 1))
                .Bind(v => Result<int>.Success(v + 1))
                .Bind(v => Result<string>.Success(v.ToString()));

            actual.Should().BeEquivalentTo(expected);
        }

        [Fact]
        public void CanChangeVoidResultToResult()
        {
            var result = Result.Success();
            var expected = Result<string>.Success("Success");

            var actual = result.Bind(_ => Result<string>.Success("Success"));

            actual.Should().BeEquivalentTo(expected);
        }

        [Fact]
        public void CanBindResultToVoidResult()
        {
            var result = Result<int>.Success(123);
            var expected = Result.Success();

            var actual = result.Bind(_ => Result.Success());

            actual.Should().BeEquivalentTo(expected);
        }

        [Fact]
        public void ShouldProduceComplexTypeResultFromSuccessAnonymousFunction()
        {
            var foo = new Foo("Bar");
            var result = Result<Foo>.Success(foo);
            var expected = Result<FooDto>.Success(new FooDto(foo.Bar));

            var actual = result.Bind(foo => Result<FooDto>.Success(new FooDto(foo.Bar)));

            actual.Should().BeEquivalentTo(expected);
        }

        [Fact]
        public void ShouldProduceComplexTypeResultFromSuccessWithMethod()
        {
            var foo = new Foo("Bar");
            var result = Result<Foo>.Success(foo);
            var expected = Result<FooDto>.Success(FooDto.CreateFromFooResult(foo));

            var actual = result.Bind(FooDto.CreateFromFooResult);

            actual.Should().BeEquivalentTo(expected);
        }

        [Fact]
        public void ShouldProduceCreatedResultFromCreated()
        {
            int createdValue = 123;
            var result = Result<int>.Created(createdValue);
            var expected = Result<string>.Created(createdValue.ToString());

            var actual = result.Bind(val => Result<string>.Created(val.ToString()));

            actual.Status.Should().Be(ResultStatus.Created);
            actual.Value.Should().Be(expected.Value);
        }

        [Fact]
        public void ShouldProduceComplexTypeResultFromCreatedAnonymously()
        {
            var foo = new Foo("Bar");
            var result = Result<Foo>.Created(foo);
            var expected = Result<FooDto>.Created(new FooDto(foo.Bar));

            var actual = result.Bind(foo => Result<FooDto>.Created(new FooDto(foo.Bar)));

            actual.Should().BeEquivalentTo(expected);
        }

        [Fact]
        public void ShouldProduceComplexTypeResultFromCreatedWithMethod()
        {
            var foo = new Foo("Bar");
            var result = Result<Foo>.Created(foo);
            var expected = Result<FooDto>.Created(FooDto.CreateFromFooResult(foo));

            var actual = result.Bind(FooDto.CreateFromFooCreatedResult);

            actual.Should().BeEquivalentTo(expected);
        }

        [Fact]
        public void ShouldProduceNotFound()
        {
            var result = Result<int>.NotFound();

            var actual = result.Bind(val => Result<string>.Success(val.ToString()));

            actual.Status.Should().Be(ResultStatus.NotFound);
            actual.Value.Should().BeNull();
        }

        [Fact]
        public void ShouldProduceNotFoundWithError()
        {
            string expectedMessage = "Some integer not found";
            var result = Result<int>.NotFound(expectedMessage);

            var actual = result.Bind(val => Result<string>.Success(val.ToString()));

            actual.Status.Should().Be(ResultStatus.NotFound);
            actual.Errors.Single().Should().Be(expectedMessage);
        }

        [Fact]
        public void ShouldProduceUnauthorized()
        {
            var result = Result<int>.Unauthorized();

            var actual = result.Bind(val => Result<string>.Success(val.ToString()));

            actual.Status.Should().Be(ResultStatus.Unauthorized);
        }

        [Fact]
        public void ShouldProduceForbidden()
        {
            var result = Result<int>.Forbidden();

            var actual = result.Bind(val => Result<string>.Success(val.ToString()));

            actual.Status.Should().Be(ResultStatus.Forbidden);
        }

        [Fact]
        public void ShouldProduceInvalidWithValidationErrors()
        {
            var validationErrors = new List<ValidationError>
            {
                new() { ErrorMessage = "Validation Error 1" },
                new() { ErrorMessage = "Validation Error 2" }
            };
            var result = Result<int>.Invalid(validationErrors);

            var actual = result.Bind(val => Result<string>.Success(val.ToString()));

            actual.Status.Should().Be(ResultStatus.Invalid);
            actual.ValidationErrors.Should().BeEquivalentTo(validationErrors);
        }

        [Fact]
        public void ShouldProduceInvalidWithoutValidationErrors()
        {
            var validationErrors = new List<ValidationError>();
            var result = Result<int>.Invalid(validationErrors);

            var actual = result.Bind(val => Result<string>.Success(val.ToString()));

            actual.Status.Should().Be(ResultStatus.Invalid);
            actual.ValidationErrors.Should().BeEmpty();
        }

        [Fact]
        public void ShouldProduceErrorResultWithErrors()
        {
            var errorList = new ErrorList(new[] { "Error 1", "Error 2" }, default);
            var result = Result<int>.Error(errorList);

            var actual = result.Bind(val => Result<string>.Success(val.ToString()));

            actual.Status.Should().Be(ResultStatus.Error);
            actual.Errors.Should().BeEquivalentTo(errorList.ErrorMessages);
        }

        [Fact]
        public void ShouldProduceErrorResultWithNoErrors()
        {
            var result = Result<int>.Error();

            var actual = result.Bind(val => Result<string>.Success(val.ToString()));

            actual.Status.Should().Be(ResultStatus.Error);
            actual.Errors.Should().BeEmpty();
            actual.CorrelationId.Should().BeEmpty();
        }

        [Fact]
        public void ShouldProduceConflict()
        {
            var result = Result<int>.Conflict();

            var actual = result.Bind(val => Result<string>.Success(val.ToString()));

            actual.Status.Should().Be(ResultStatus.Conflict);
        }

        [Fact]
        public void ShouldProduceConflictWithError()
        {
            string expectedMessage = "Some conflict";
            var result = Result<int>.Conflict(expectedMessage);

            var actual = result.Bind(val => Result<string>.Success(val.ToString()));

            actual.Status.Should().Be(ResultStatus.Conflict);
            actual.Errors.Single().Should().Be(expectedMessage);
        }

        [Fact]
        public void ShouldProduceUnavailableWithError()
        {
            string expectedMessage = "Something unavailable";
            var result = Result<int>.Unavailable(expectedMessage);

            var actual = result.Bind(val => Result<string>.Success(val.ToString()));

            actual.Status.Should().Be(ResultStatus.Unavailable);
            actual.Errors.Single().Should().Be(expectedMessage);
        }

        [Fact]
        public void ShouldProduceCriticalErrorWithError()
        {
            string expectedMessage = "Some critical error";
            var result = Result<int>.CriticalError(expectedMessage);

            var actual = result.Bind(val => Result<string>.Success(val.ToString()));

            actual.Status.Should().Be(ResultStatus.CriticalError);
            actual.Errors.Single().Should().Be(expectedMessage);
        }

        [Fact]
        public void ShouldProduceForbiddenWithError()
        {
            string expectedMessage = "You are forbidden";
            var result = Result<int>.Forbidden(expectedMessage);

            var actual = result.Bind(val => Result<string>.Success(val.ToString()));

            actual.Status.Should().Be(ResultStatus.Forbidden);
            actual.Errors.Single().Should().Be(expectedMessage);
        }

        [Fact]
        public void ShouldProduceUnauthorizedWithError()
        {
            string expectedMessage = "You are unauthorized";
            var result = Result<int>.Unauthorized(expectedMessage);

            var actual = result.Bind(val => Result<string>.Success(val.ToString()));

            actual.Status.Should().Be(ResultStatus.Unauthorized);
            actual.Errors.Single().Should().Be(expectedMessage);
        }

        [Fact]
        public void ShouldProduceNoContentWithoutAnyContent()
        {
            var result = Result<int>.NoContent();

            var actual = result.Bind(val => Result<string>.Success(val.ToString()));

            actual.Status.Should().Be(ResultStatus.NoContent);
            actual.Value.Should().BeNull();
            actual.Errors.Should().BeEmpty();
            actual.ValidationErrors.Should().BeEmpty();
        }

        [Fact]
        public void ShouldPropagateErrorFromBindFunction()
        {
            int successValue = 123;
            var result = Result<int>.Success(successValue);
            string expectedError = "Bind function failed";

            var actual = result.Bind(val => Result<string>.Error(expectedError));

            actual.Status.Should().Be(ResultStatus.Error);
            actual.Errors.Single().Should().Be(expectedError);
            actual.Value.Should().BeNull();
        }

        [Fact]
        public void ShouldHandleNestedResultsInBind()
        {
            int successValue = 123;
            var result = Result<int>.Success(successValue);

            var actual = result.Bind(val =>
            {
                if (val > 1)
                {
                    return Result<string>.Success("Value is greater than 1");
                }
                else
                {
                    return Result<string>.Error("Value is less than or equal to 1");
                }
            });

            actual.Status.Should().Be(ResultStatus.Ok);
            actual.Value.Should().Be("Value is greater than 1");
        }

        [Fact]
        public void ShouldHandleValidationErrorsFromBindFunction()
        {
            int successValue = 0;
            var result = Result<int>.Success(successValue);
            var validationErrors = new List<ValidationError>
            {
                new() { ErrorMessage = "Value must be greater than 1" }
            };

            var actual = result.Bind(val =>
            {
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

        private record Foo(string Bar);

        private class FooDto
        {
            public string Bar { get; set; }

            public FooDto(string bar)
            {
                Bar = bar;
            }

            public static Result<FooDto> CreateFromFooResult(Foo foo)
            {
                return Result<FooDto>.Success(new FooDto(foo.Bar));
            }

            public static Result<FooDto> CreateFromFooCreatedResult(Foo foo)
            {
                return Result<FooDto>.Created(new FooDto(foo.Bar));
            }
        }
    }
}
