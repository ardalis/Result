using FluentAssertions;
using System.Collections.Generic;
using Xunit;

namespace Ardalis.Result.UnitTests
{
    public class ResultVoidMap
    {
        [Fact]
        public void ShouldProduceSuccessWithValueGivenResultOfVoid()
        {
            var result = Result.Success();

            var actual = result.Map(_ => "Success");

            actual.Value.Should().Be("Success");
        }

        [Fact]
        public void ShouldProduceNotFound()
        {
            var result = Result.NotFound();

            var actual = result.Map(_ => "This should be ignored");

            actual.Status.Should().Be(ResultStatus.NotFound);
            actual.Value.Should().BeNull();
        }

        [Fact]
        public void ShouldProduceUnauthorized()
        {
            var result = Result.Unauthorized();

            var actual = result.Map(_ => "This should be ignored");

            actual.Status.Should().Be(ResultStatus.Unauthorized);
            actual.Value.Should().BeNull();
        }

        [Fact]
        public void ShouldProduceForbidden()
        {
            var result = Result.Forbidden();

            var actual = result.Map(_ => "This should be ignored");

            actual.Status.Should().Be(ResultStatus.Forbidden);
            actual.Value.Should().BeNull();
        }

        [Fact]
        public void ShouldProduceInvalidWithEmptyList()
        {
            var result = Result.Invalid(new List<ValidationError>());

            var actual = result.Map(_ => "This should be ignored");

            actual.Status.Should().Be(ResultStatus.Invalid);
            actual.Value.Should().BeNull();
        }

        [Fact]
        public void ShouldProduceInvalid()
        {
            var result = Result.Invalid(new ValidationError());

            var actual = result.Map(_ => "This should be ignored");

            actual.Status.Should().Be(ResultStatus.Invalid);
            actual.Value.Should().BeNull();
        }

        [Fact]
        public void ShouldProduceError()
        {
            var result = Result.Error();

            var actual = result.Map(_ => "This should be ignored");

            actual.Status.Should().Be(ResultStatus.Error);
            actual.Value.Should().BeNull();
        }
        
        [Fact]
        public void ShouldProduceConflict()
        {
            var result = Result.Conflict();

            var actual = result.Map(_ => "This should be ignored");

            actual.Status.Should().Be(ResultStatus.Conflict);
            actual.Value.Should().BeNull();
        }

        [Fact]
        public void ShouldProduceUnavailable()
        {
            var result = Result.Unavailable();

            var actual = result.Map(_ => "This should be ignored");

            actual.Status.Should().Be(ResultStatus.Unavailable);
            actual.Value.Should().BeNull();
        }
    }
}
