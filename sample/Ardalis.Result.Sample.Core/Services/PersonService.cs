﻿using Ardalis.Result.Sample.Core.Model;
using Ardalis.Result.Sample.Core.Validators;
using Ardalis.Result.FluentValidation;

namespace Ardalis.Result.Sample.Core.Services
{
    public class PersonService
    {
        public Result<Person> Create(string firstName, string lastName)
        {
            var person = new Person();
            person.Forename = firstName;
            person.Surname = lastName;

            var validator = new PersonValidator();

            var result = validator.Validate(person);
            if (!result.IsValid)
            {
                return Result<Person>.Invalid(result.AsErrors());
            }

            return Result<Person>.Success(person);
        }
    }

}

