using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace Ardalis.Result.UnitTests
{
    public class ResultMap
    {
        [Fact]
        public void ShouldProduceReturnValueFromSuccess()
        {
            int successValue = 123;
            var result = Result<int>.Success(successValue);
            string expected = successValue.ToString();

            var actual = result.Map(val => val.ToString());

            expected.Should().BeEquivalentTo(actual.Value);
        }

        [Fact]
        public void ShouldProduceComplexTypeReturnValueFromSuccessAnonymously()
        {
            var foo = new Foo("Bar");
            var result = Result<Foo>.Success(foo);

            var actual = result.Map(foo => new FooDto(foo.Bar));

            actual.Value.Bar.Should().Be(foo.Bar);
        }

        [Fact]
        public void ShouldProduceComplexTypeReturnValueFromSuccessWithMethod()
        {
            var foo = new Foo("Bar");
            var result = Result<Foo>.Success(foo);

            var actual = result.Map(FooDto.CreateFromFoo);

            actual.Value.Bar.Should().Be(foo.Bar);
        }

        [Fact]
        public void ShouldProduceReturnValueFromCreated()
        {
            int createdValue = 123;
            var result = Result<int>.Created(createdValue);
            string expected = createdValue.ToString();
            
            var actual = result.Map(val => val.ToString());

            expected.Should().BeEquivalentTo(actual.Value);
        }
        
        [Fact]
        public void ShouldProduceComplexTypeReturnValueFromCreatedAnonymously()
        {
            var foo = new Foo("Bar");
            var result = Result<Foo>.Created(foo);

            var actual = result.Map(foo => new FooDto(foo.Bar));

            actual.Value.Bar.Should().Be(foo.Bar);
        }
        
        [Fact]
        public void ShouldProduceComplexTypeReturnValueFromCreatedWithMethod()
        {
            var foo = new Foo("Bar");
            var result = Result<Foo>.Created(foo);

            var actual = result.Map(FooDto.CreateFromFoo);

            actual.Value.Bar.Should().Be(foo.Bar);
        }

        [Fact]
        public void ShouldProduceNotFound()
        {
            var result = Result<int>.NotFound();

            var actual = result.Map(val => val.ToString());

            actual.Status.Should().Be(ResultStatus.NotFound);
        }

        [Fact]
        public void ShouldProduceNotFoundWithError()
        {
            string expectedMessage = "Some integer not found";
            var result = Result<int>.NotFound(expectedMessage);

            var actual = result.Map(val => val.ToString());

            actual.Status.Should().Be(ResultStatus.NotFound);
            actual.Errors.Single().Should().Be(expectedMessage);
        }

        [Fact]
        public void ShouldProduceUnauthorized()
        {
            var result = Result<int>.Unauthorized();

            var actual = result.Map(val => val.ToString());

            actual.Status.Should().Be(ResultStatus.Unauthorized);
        }

        [Fact]
        public void ShouldProduceForbidden()
        {
            var result = Result<int>.Forbidden();

            var actual = result.Map(val => val.ToString());

            actual.Status.Should().Be(ResultStatus.Forbidden);
        }

        [Fact]
        public void ShouldProduceInvalidWithValidationErrors()
        {
            var validationErrors = new List<ValidationError>
            {
                new ValidationError
                {
                    ErrorMessage = "Validation Error 1"
                },
                new ValidationError
                {
                    ErrorMessage = "Validation Error 2"
                }
            };
            var result = Result<int>.Invalid(validationErrors);

            var actual = result.Map(val => val.ToString());

            actual.Status.Should().Be(ResultStatus.Invalid);
            actual.ValidationErrors.Should().BeEquivalentTo(validationErrors);
        }

        [Fact]
        public void ShouldProduceInvalidWithoutValidationErrors()
        {
            var validationErrors = new List<ValidationError>();
            var result = Result<int>.Invalid(validationErrors);

            var actual = result.Map(val => val.ToString());

            actual.Status.Should().Be(ResultStatus.Invalid);
            actual.ValidationErrors.Should().BeEquivalentTo(validationErrors);
        }

        [Fact]
        public void ShouldProduceErrorResultWithErrors()
        {
            var errors = new string[] { "Error 1", "Error 2" };
            var result = Result<int>.Error(errors);

            var actual = result.Map(val => val.ToString());

            actual.Status.Should().Be(ResultStatus.Error);
            actual.Errors.Should().BeEquivalentTo(errors);
        }

        [Fact]
        public void ShouldProduceErrorResultWithNoErrors()
        {
            var errors = Array.Empty<string>();
            var result = Result<int>.Error(errors);

            var actual = result.Map(val => val.ToString());

            actual.Status.Should().Be(ResultStatus.Error);
            actual.Errors.Should().BeEquivalentTo(errors);
        }
        
        [Fact]
        public void ShouldProduceConflict()
        {
            var result = Result<int>.Conflict();

            var actual = result.Map(val => val.ToString());

            actual.Status.Should().Be(ResultStatus.Conflict);
        }

        [Fact]
        public void ShouldProduceConflictWithError()
        {
            string expectedMessage = "Some conflict";
            var result = Result<int>.Conflict(expectedMessage);

            var actual = result.Map(val => val.ToString());

            actual.Status.Should().Be(ResultStatus.Conflict);
            actual.Errors.Single().Should().Be(expectedMessage);
        }

        [Fact]
        public void ShouldProduceUnavailableWithError()
        {
            string expectedMessage = "Something unavailable";
            var result = Result<int>.Unavailable(expectedMessage);

            var actual = result.Map(val => val.ToString());

            actual.Status.Should().Be(ResultStatus.Unavailable);
            actual.Errors.Single().Should().Be(expectedMessage);
        }

        [Fact]
        public void ShouldProduceCriticalErrorWithError()
        {
            string expectedMessage = "Some critical error";
            var result = Result<int>.CriticalError(expectedMessage);

            var actual = result.Map(val => val.ToString());

            actual.Status.Should().Be(ResultStatus.CriticalError);
            actual.Errors.Single().Should().Be(expectedMessage);
        }

        [Fact]
        public void ShouldProductNoContentWithoutAnyContent()
        {
            var result = Result<int>.NoContent();

            var actual = result.Map(val => val.ToString());

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

            public static FooDto CreateFromFoo(Foo foo)
            {
                return new FooDto(foo.Bar);
            }
        }
    }
}
