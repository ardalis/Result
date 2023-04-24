using Ardalis.Result.Sample.Core.Model;
using Ardalis.Result.Sample.Core.Validators;
using Ardalis.Result.FluentValidation;
using System.Linq;

namespace Ardalis.Result.Sample.Core.Services
{
    public class PersonService
    {
        private readonly int[] _knownIds = new [] { 1 };
        private readonly Person _existPerson = new() { Forename = "John", Surname = "Smith" };

        public Result<Person> Create(string firstName, string lastName)
        {
            var person = new Person
            {
                Forename = firstName,
                Surname = lastName
            };

            var validator = new PersonValidator();

            var result = validator.Validate(person);
            if (!result.IsValid)
            {
                return Result.Invalid(result.AsErrors());
            }

            if (person.Forename == _existPerson.Forename && person.Surname == _existPerson.Surname)
            {
                return Result.Conflict($"Person ({person.Forename} {person.Surname}) is exist");
            }

            return Result.Success(person);
        }

        public Result Remove(int id)
        {
            if (!_knownIds.Any(knownId => knownId == id))
            {
                return Result.NotFound("Person Not Found");
            }

            //Pretend removing person

            return Result.Success();
        }
    }

}

