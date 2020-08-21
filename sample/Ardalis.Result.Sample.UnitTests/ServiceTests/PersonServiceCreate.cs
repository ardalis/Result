using Ardalis.Sample.Core.Services;
using FluentAssertions;
using Xunit;

namespace Ardalis.Result.Sample.UnitTests.ServiceTests
{
    public class PersonServiceCreate
    {
        [Fact]
        public void ReturnsInvalidResultGivenEmptyNames()
        {
            var service = new PersonService();

            var result = service.Create("", "");

            result.Status.Should().Be(ResultStatus.Invalid);
            result.ValidationErrors.Count.Should().Be(2);
        }

        [Fact]
        public void ReturnsInvalidResultWith2ErrorsGivenSomeLongNameSurname()
        {
            var service = new PersonService();

            var result = service.Create("Steve", "SomeLongName");

            result.Status.Should().Be(ResultStatus.Invalid);
            result.ValidationErrors.Count.Should().Be(2);
        }

    }
}
