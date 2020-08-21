using Ardalis.Sample.Core.Model;
using FluentValidation;

namespace Ardalis.Sample.Core.Validators
{
    public class PersonValidator : AbstractValidator<Person>
    {
        public PersonValidator()
        {
            RuleFor(x => x.Surname)
                .NotEmpty()
                .MaximumLength(10)
                .NotEqual("SomeLongName");
            RuleFor(x => x.Forename).NotEmpty().WithMessage("Please specify a first name");
        }
    }
}
