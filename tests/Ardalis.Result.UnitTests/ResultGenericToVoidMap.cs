using FluentAssertions;
using Xunit;

namespace Ardalis.Result.UnitTests;

public class ResultGenericToVoidMap
{
    [Fact]
    public void ShouldProduceSuccess()
    {
        var result = Result<int>.Success(1);

        var actual = result.Map();

        actual.Status.Should().Be(ResultStatus.Ok);
    }

    [Fact]
    public void ShouldProduceNotFound()
    {
        var result = Result<int>.NotFound();

        var actual = result.Map();

        actual.Status.Should().Be(ResultStatus.NotFound);
    }

    [Fact]
    public void ShouldProduceUnauthorized()
    {
        var result = Result<int>.Unauthorized();

        var actual = result.Map();

        actual.Status.Should().Be(ResultStatus.Unauthorized);
    }

    [Fact]
    public void ShouldProduceForbidden()
    {
        var result = Result<int>.Forbidden();

        var actual = result.Map();

        actual.Status.Should().Be(ResultStatus.Forbidden);
    }

    [Fact]
    public void ShouldProduceInvalid()
    {
        var result = Result<int>.Invalid(new ValidationError());

        var actual = result.Map();

        actual.Status.Should().Be(ResultStatus.Invalid);
    }

    [Fact]
    public void ShouldProduceError()
    {
        var result = Result<int>.Error();

        var actual = result.Map();

        actual.Status.Should().Be(ResultStatus.Error);
    }

    [Fact]
    public void ShouldProduceConflict()
    {
        var result = Result<int>.Conflict();

        var actual = result.Map();

        actual.Status.Should().Be(ResultStatus.Conflict);
    }

    [Fact]
    public void ShouldProduceCriticalError()
    {
        var result = Result<int>.CriticalError();

        var actual = result.Map();

        actual.Status.Should().Be(ResultStatus.CriticalError);
    }

    [Fact]
    public void ShouldProduceUnavailable()
    {
        var result = Result<int>.Unavailable();

        var actual = result.Map();

        actual.Status.Should().Be(ResultStatus.Unavailable);
    }

    [Fact]
    public void ShouldProduceNoContent()
    {
        var result = Result<int>.NoContent();

        var actual = result.Map();

        actual.Status.Should().Be(ResultStatus.NoContent);
    }
}