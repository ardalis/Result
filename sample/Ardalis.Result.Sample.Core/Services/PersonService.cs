﻿using Ardalis.Result.Sample.Core.Model;
using Ardalis.Result.Sample.Core.Validators;
using Ardalis.Result.FluentValidation;
using System.Linq;

namespace Ardalis.Result.Sample.Core.Services
{
    public class PersonService
    {
        private readonly int[] _knownIds = new [] { 1 };

        public Result<Person> Create(string firstName, string lastName)
        {
            var person = new Person();
            person.Forename = firstName;
            person.Surname = lastName;

            var validator = new PersonValidator();

            var result = validator.Validate(person);
            if (!result.IsValid)
            {
                return Result.Invalid(result.AsErrors());
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

