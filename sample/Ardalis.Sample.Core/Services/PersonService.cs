using Ardalis.Result;
using Ardalis.Sample.Core.Model;
using Ardalis.Sample.Core.Validators;
using FluentValidation.Results;
using System;
using System.Collections.Generic;

namespace Ardalis.Sample.Core.Services
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

namespace FluentValidation.Results
{
    public static class FluentValidationResultExtensions
    {
        public static List<ValidationError> AsErrors(this ValidationResult valResult)
        {
            var resultErrors = new List<ValidationError>();

            foreach (var valFailure in valResult.Errors)
            {
                resultErrors.Add(new ValidationError()
                {
                    Severity = FromSeverity(valFailure.Severity),
                    ErrorMessage = valFailure.ErrorMessage,
                    Identifier = valFailure.PropertyName
                }) ;
            }

            return resultErrors;
        }

        public static ValidationSeverity FromSeverity(Severity severity)
        {
            switch(severity)
            {
                case Severity.Error: return ValidationSeverity.Error;
                case Severity.Warning: return ValidationSeverity.Warning;
                case Severity.Info: return ValidationSeverity.Info;
                default: throw new ArgumentOutOfRangeException(nameof(severity), "Unexpected Severity");
            }
        }
    }

}

