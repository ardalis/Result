using Ardalis.Result.Sample.Core.Model;
using Ardalis.Result.Sample.Core.Validators;
using FluentAssertions;
using System.Linq;
using Xunit;

namespace Ardalis.Result.Sample.UnitTests.ValidatorTests;

public class PersonValidatorValidate
{
    [Fact]
    public void ReturnsTrueGivenValidPerson()
    {
        var person = new Person()
        {
            Surname = "Testname",
            Forename = "Testname2"
        };

        var validator = new PersonValidator();

        var result = validator.Validate(person);

        Assert.True(result.IsValid);
    }

    [Fact]
    public void ReturnsFalseGivenPersonWithNoSurname()
    {
        var person = new Person()
        {
            Forename = "TestForename"
        };

        var validator = new PersonValidator();

        var result = validator.Validate(person);

        Assert.False(result.IsValid);
        Assert.True(result.Errors.Any());
    }

    [Fact]
    public void ReturnsFalseGivenPersonWithNoForename()
    {
        var person = new Person()
        {
            Surname = "TestSurname",
        };

        var validator = new PersonValidator();

        var result = validator.Validate(person);

        Assert.False(result.IsValid);
        Assert.True(result.Errors.Any());
    }

    [Fact]
    public void ReturnsFalseGivenPersonWithSomeLongNameForSurname()
    {
        var person = new Person()
        {
            Surname = "SomeLongName",
            Forename = "Steve"
        };

        var validator = new PersonValidator();

        var result = validator.Validate(person);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().HaveCount(2);
    }
}
