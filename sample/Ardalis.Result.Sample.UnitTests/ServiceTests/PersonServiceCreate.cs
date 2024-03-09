using System.Linq;
using Ardalis.Result.Sample.Core.Services;
using FluentAssertions;
using Xunit;

namespace Ardalis.Result.Sample.UnitTests.ServiceTests;

public class PersonServiceCreate
{
    [Fact]
    public void ReturnsInvalidResultGivenEmptyNames()
    {
        var service = new PersonService();

        var result = service.Create("", "");

        result.Status.Should().Be(ResultStatus.Invalid);
        result.ValidationErrors.Should().HaveCount(2);
    }

    [Fact]
    public void ReturnsInvalidResultWith2ErrorsGivenSomeLongNameSurname()
    {
        var service = new PersonService();

        var result = service.Create("Steve", "SomeLongName");

        result.Status.Should().Be(ResultStatus.Invalid);
        result.ValidationErrors.Should().HaveCount(2);
    }

    [Fact]
    public void ReturnsConflictResultGivenExistPerson()
    {
        var service = new PersonService();
        string firstName = "John";
        string lastName = "Smith";

        var result = service.Create(firstName, lastName);

        result.Status.Should().Be(ResultStatus.Conflict);
        result.Errors.Single().Should().Be($"Person ({firstName} {lastName}) is exist");
    }
}
