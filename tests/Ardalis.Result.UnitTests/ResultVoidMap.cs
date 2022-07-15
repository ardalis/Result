using FluentAssertions;
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
            var result = Result<int>.Forbidden();

            var actual = result.Map(_ => "This should be ignored");

            actual.Status.Should().Be(ResultStatus.Forbidden);
            actual.Value.Should().BeNull();
        }

        [Fact]
        public void ShouldProduceInvalid()
        {
            var result = Result<int>.Invalid(new());

            var actual = result.Map(_ => "This should be ignored");

            actual.Status.Should().Be(ResultStatus.Invalid);
            actual.Value.Should().BeNull();
        }

        [Fact]
        public void ShouldProduceError()
        {
            var result = Result<int>.Error();

            var actual = result.Map(_ => "This should be ignored");

            actual.Status.Should().Be(ResultStatus.Error);
            actual.Value.Should().BeNull();
        }
    }
}
