using System.Linq;
using Ardalis.Result.Sample.Core.Services;
using FluentAssertions;
using Xunit;

namespace Ardalis.Result.Sample.UnitTests.ServiceTests;

public class BadResultService_ReturnResultWithError
{
   [Fact]
    public void ReturnsErrorResultGivenMessage()
    {
        var service = new BadResultService();

        var result = service.ReturnErrorWithMessage("Some error message");

        result.Status.Should().Be(ResultStatus.Error);
        result.Errors.Single().Should().Be("Some error message");
    }
}
