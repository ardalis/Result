using Ardalis.Result.Sample.Core.Services;
using FluentAssertions;
using Xunit;

namespace Ardalis.Result.Sample.UnitTests.ServiceTests;

public class PersonServiceRemove
{
    [Fact]
    public void ReturnsSuccessResultGivenKnownId()
    {
        var service = new PersonService();

        var result = service.Remove(1);

        result.Status.Should().Be(ResultStatus.Ok);
    }

    [Fact]
    public void ReturnsNotFoundResultGivenUnknownId()
    {
        var service = new PersonService();

        var result = service.Remove(2);

        result.Status.Should().Be(ResultStatus.NotFound);
    }
}
