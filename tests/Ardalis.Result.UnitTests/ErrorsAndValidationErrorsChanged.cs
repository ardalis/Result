using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Ardalis.Result.UnitTests
{
    public class ErrorsAndValidationErrorsChanged
    {
        [Fact]
        public void ValidationErrorsChanged()
        {
            Result result = Result.Success();
            result.IsSuccess.Should().Be(true);
            result.Status.Should().Be(ResultStatus.Ok);

            // Adding a validation error
            result.ValidationErrors.Add(new ValidationError("Error 1"));
            result.IsSuccess.Should().Be(false);
            result.Status.Should().Be(ResultStatus.Invalid);

            // clearing the validation errors
            result.ValidationErrors.Clear();
            result.IsSuccess.Should().Be(true);
            result.Status.Should().Be(ResultStatus.Ok);
        }

        [Fact]
        public void ErrorsChanged()
        {
            Result result = Result.Success();
            result.IsSuccess.Should().Be(true);
            result.Status.Should().Be(ResultStatus.Ok);

            // Adding a validation error
            result.Errors.Add("Error 1");
            result.IsSuccess.Should().Be(false);
            result.Status.Should().Be(ResultStatus.Error);

            // clearing the validation errors
            result.Errors.Clear();
            result.IsSuccess.Should().Be(true);
            result.Status.Should().Be(ResultStatus.Ok);
        }

        [Fact]
        public void ErrorAndValidationErrorChanged()
        {
            Result result = Result.Success();
            result.IsSuccess.Should().Be(true);
            result.Status.Should().Be(ResultStatus.Ok);

            // Adding a validation error
            result.ValidationErrors.Add(new ValidationError("Error 1"));
            result.IsSuccess.Should().Be(false);
            result.Status.Should().Be(ResultStatus.Invalid);

            // Adding an error
            result.Errors.Add("Error 1");
            result.IsSuccess.Should().Be(false);
            result.Status.Should().Be(ResultStatus.Error);

            // clearing the errors (still remain one Validation Error, so it's Invalid)
            result.Errors.Clear();
            result.IsSuccess.Should().Be(false);
            result.Status.Should().Be(ResultStatus.Invalid);

            result.ValidationErrors.Clear();
            result.IsSuccess.Should().Be(true);
            result.Status.Should().Be(ResultStatus.Ok);
        }

        [Fact]
        public void SettingTheValidationErrorsProperty()
        {
            Result result = Result.Success();
            result.IsSuccess.Should().Be(true);
            result.Status.Should().Be(ResultStatus.Ok);

            List<ValidationError> validationErrors = new List<ValidationError>();
            validationErrors.Add(new ValidationError("Error 1"));
            validationErrors.Add(new ValidationError("Error 2"));

            result.ValidationErrors = new ObservableCollection<ValidationError>(validationErrors);

            result.IsSuccess.Should().Be(false);
            result.Status.Should().Be(ResultStatus.Invalid);
        }

        [Fact]
        public void SettingTheErrorsProperty()
        {
            Result result = Result.Success();
            result.IsSuccess.Should().Be(true);
            result.Status.Should().Be(ResultStatus.Ok);

            List<string> errors = new List<string>();
            errors.Add("Error 1");
            errors.Add("Error 2");

            result.Errors = new ObservableCollection<string>(errors);

            result.IsSuccess.Should().Be(false);
            result.Status.Should().Be(ResultStatus.Error);
        }

        /// <summary>
        /// Checks if the initial state is correct after addind and removing Errors and ValidationErros from the result
        /// </summary>
        [Fact]
        public void CheckOkInitialStateAfterChangeErrorsAndValidationErrors()
        {
            Result result = Result.Success(); // Initial state
            result.IsSuccess.Should().Be(true);
            result.Status.Should().Be(ResultStatus.Ok);

            result.ValidationErrors.Add(new ValidationError("Validation Error1"));
            result.IsSuccess.Should().Be(false);
            result.Status.Should().Be(ResultStatus.Invalid);

            result.Errors.Add("Error1");
            result.IsSuccess.Should().Be(false);
            result.Status.Should().Be(ResultStatus.Error);

            result.Errors.Clear();
            result.IsSuccess.Should().Be(false);
            result.Status.Should().Be(ResultStatus.Invalid);

            result.ValidationErrors.Clear();
            result.IsSuccess.Should().Be(true);
            result.Status.Should().Be(ResultStatus.Ok); // Initial state
        }

        /// <summary>
        /// Checks if the initial state is correct after addind and removing Errors and ValidationErros from the result
        /// </summary>
        [Fact]
        public void CheckErrorInitialStateAfterChangeErrorsAndValidationErrors()
        {
            Result result = Result.Error(); // Initial state
            result.IsSuccess.Should().Be(false);
            result.Status.Should().Be(ResultStatus.Error);

            result.ValidationErrors.Add(new ValidationError("Validation Error1"));
            result.IsSuccess.Should().Be(false);
            result.Status.Should().Be(ResultStatus.Invalid);

            result.Errors.Add("Error1");
            result.IsSuccess.Should().Be(false);
            result.Status.Should().Be(ResultStatus.Error);

            result.Errors.Clear();
            result.IsSuccess.Should().Be(false);
            result.Status.Should().Be(ResultStatus.Invalid);

            result.ValidationErrors.Clear();
            result.IsSuccess.Should().Be(false);
            result.Status.Should().Be(ResultStatus.Error); // Initial state
        }

        /// <summary>
        /// Checks if the initial state is correct after addind and removing Errors and ValidationErros from the result
        /// </summary>
        [Fact]
        public void CheckInvalidInitialStateAfterChangeErrorsAndValidationErrors()
        {
            Result result = Result.Invalid(); // Initial State
            result.IsSuccess.Should().Be(false);
            result.Status.Should().Be(ResultStatus.Invalid);

            result.ValidationErrors.Add(new ValidationError("Validation Error1"));
            result.IsSuccess.Should().Be(false);
            result.Status.Should().Be(ResultStatus.Invalid);

            result.Errors.Add("Error1");
            result.IsSuccess.Should().Be(false);
            result.Status.Should().Be(ResultStatus.Error);

            result.Errors.Clear();
            result.IsSuccess.Should().Be(false);
            result.Status.Should().Be(ResultStatus.Invalid);

            result.ValidationErrors.Clear();
            result.IsSuccess.Should().Be(false);
            result.Status.Should().Be(ResultStatus.Invalid); // Initial state 
        }

        /// <summary>
        /// Checks if the initial state is correct after addind and removing Errors and ValidationErros from the result
        /// </summary>
        [Fact]
        public void CheckConflictInitialStateAfterChangeErrorsAndValidationErrors()
        {
            Result result = Result.Conflict(); // Initial State
            result.IsSuccess.Should().Be(false);
            result.Status.Should().Be(ResultStatus.Conflict);

            result.ValidationErrors.Add(new ValidationError("Validation Error1"));
            result.IsSuccess.Should().Be(false);
            result.Status.Should().Be(ResultStatus.Conflict); // Should not be changed?

            result.Errors.Add("Error1");
            result.IsSuccess.Should().Be(false);
            result.Status.Should().Be(ResultStatus.Conflict); // Should not be changed?

            result.Errors.Clear();
            result.IsSuccess.Should().Be(false);
            result.Status.Should().Be(ResultStatus.Conflict); // Should not be changed?

            result.ValidationErrors.Clear();
            result.IsSuccess.Should().Be(false);
            result.Status.Should().Be(ResultStatus.Conflict); // Initial state 
        }

        /// <summary>
        /// Checks if the initial state is correct after addind and removing Errors and ValidationErros from the result
        /// </summary>
        [Fact]
        public void CheckUnavailableInitialStateAfterChangeErrorsAndValidationErrors()
        {
            Result result = Result.Unavailable(); // Initial State
            result.IsSuccess.Should().Be(false);
            result.Status.Should().Be(ResultStatus.Unavailable);

            result.ValidationErrors.Add(new ValidationError("Validation Error1"));
            result.IsSuccess.Should().Be(false);
            result.Status.Should().Be(ResultStatus.Unavailable); // Should not be changed?

            result.Errors.Add("Error1");
            result.IsSuccess.Should().Be(false);
            result.Status.Should().Be(ResultStatus.Unavailable); // Should not be changed?

            result.Errors.Clear();
            result.IsSuccess.Should().Be(false);
            result.Status.Should().Be(ResultStatus.Unavailable); // Should not be changed?

            result.ValidationErrors.Clear();
            result.IsSuccess.Should().Be(false);
            result.Status.Should().Be(ResultStatus.Unavailable); // Initial state 
        }

        /// <summary>
        /// Checks if the initial state is correct after addind and removing Errors and ValidationErros from the result
        /// </summary>
        [Fact]
        public void CheckCriticalErrorInitialStateAfterChangeErrorsAndValidationErrors()
        {
            Result result = Result.CriticalError(); // Initial State
            result.IsSuccess.Should().Be(false);
            result.Status.Should().Be(ResultStatus.CriticalError);

            result.ValidationErrors.Add(new ValidationError("Validation Error1"));
            result.IsSuccess.Should().Be(false);
            result.Status.Should().Be(ResultStatus.CriticalError); // Should not be changed?

            result.Errors.Add("Error1");
            result.IsSuccess.Should().Be(false);
            result.Status.Should().Be(ResultStatus.CriticalError); // Should not be changed?

            result.Errors.Clear();
            result.IsSuccess.Should().Be(false);
            result.Status.Should().Be(ResultStatus.CriticalError); // Should not be changed?

            result.ValidationErrors.Clear();
            result.IsSuccess.Should().Be(false);
            result.Status.Should().Be(ResultStatus.CriticalError); // Initial state 
        }

        /// <summary>
        /// Checks if the initial state is correct after addind and removing Errors and ValidationErros from the result
        /// </summary>
        [Fact]
        public void CheckErrorWithCorrelationIdInitialStateAfterChangeErrorsAndValidationErrors()
        {
            Result result = Result.ErrorWithCorrelationId("correlationId"); // Initial State

            Result result2 = Result.ErrorWithCorrelationId("correlationId", null); // Initial State
            result.IsSuccess.Should().Be(false);
            result.Status.Should().Be(ResultStatus.Error);

            result.ValidationErrors.Add(new ValidationError("Validation Error1"));
            result.IsSuccess.Should().Be(false);
            result.Status.Should().Be(ResultStatus.Invalid); // Should change to Invalid

            result.Errors.Add("Error1");
            result.IsSuccess.Should().Be(false);
            result.Status.Should().Be(ResultStatus.Error);// Should change to Error

            result.Errors.Clear();
            result.IsSuccess.Should().Be(false);
            result.Status.Should().Be(ResultStatus.Invalid); // Should change to Invalid

            result.ValidationErrors.Clear();
            result.IsSuccess.Should().Be(false);
            result.Status.Should().Be(ResultStatus.Error); // Initial state 
        }

        /// <summary>
        /// Checks if the initial state is correct after addind and removing Errors and ValidationErros from the result
        /// </summary>
        [Fact]
        public void CheckForbidedenInitialStateAfterChangeErrorsAndValidationErrors()
        {
            Result result = Result.Forbidden(); // Initial State
            result.IsSuccess.Should().Be(false);
            result.Status.Should().Be(ResultStatus.Forbidden);

            result.ValidationErrors.Add(new ValidationError("Validation Error1"));
            result.IsSuccess.Should().Be(false);
            result.Status.Should().Be(ResultStatus.Forbidden); // Should not be changed?

            result.Errors.Add("Error1");
            result.IsSuccess.Should().Be(false);
            result.Status.Should().Be(ResultStatus.Forbidden); // Should not be changed?

            result.Errors.Clear();
            result.IsSuccess.Should().Be(false);
            result.Status.Should().Be(ResultStatus.Forbidden); // Should not be changed?

            result.ValidationErrors.Clear();
            result.IsSuccess.Should().Be(false);
            result.Status.Should().Be(ResultStatus.Forbidden); // Initial state 
        }

        /// <summary>
        /// Checks if the initial state is correct after addind and removing Errors and ValidationErros from the result
        /// </summary>
        [Fact]
        public void CheckNotFoundInitialStateAfterChangeErrorsAndValidationErrors()
        {
            Result result = Result.NotFound(); // Initial State
            result.IsSuccess.Should().Be(false);
            result.Status.Should().Be(ResultStatus.NotFound);

            result.ValidationErrors.Add(new ValidationError("Validation Error1"));
            result.IsSuccess.Should().Be(false);
            result.Status.Should().Be(ResultStatus.NotFound); // Should not be changed?

            result.Errors.Add("Error1");
            result.IsSuccess.Should().Be(false);
            result.Status.Should().Be(ResultStatus.NotFound); // Should not be changed?

            result.Errors.Clear();
            result.IsSuccess.Should().Be(false);
            result.Status.Should().Be(ResultStatus.NotFound); // Should not be changed?

            result.ValidationErrors.Clear();
            result.IsSuccess.Should().Be(false);
            result.Status.Should().Be(ResultStatus.NotFound); // Initial state 
        }

        /// <summary>
        /// Checks if the initial state is correct after addind and removing Errors and ValidationErros from the result
        /// </summary>
        [Fact]
        public void CheckUnauthorizedInitialStateAfterChangeErrorsAndValidationErrors()
        {
            Result result = Result.Unauthorized(); // Initial State
            result.IsSuccess.Should().Be(false);
            result.Status.Should().Be(ResultStatus.Unauthorized);

            result.ValidationErrors.Add(new ValidationError("Validation Error1"));
            result.IsSuccess.Should().Be(false);
            result.Status.Should().Be(ResultStatus.Unauthorized); // Should not be changed?

            result.Errors.Add("Error1");
            result.IsSuccess.Should().Be(false);
            result.Status.Should().Be(ResultStatus.Unauthorized); // Should not be changed?

            result.Errors.Clear();
            result.IsSuccess.Should().Be(false);
            result.Status.Should().Be(ResultStatus.Unauthorized); // Should not be changed?

            result.ValidationErrors.Clear();
            result.IsSuccess.Should().Be(false);
            result.Status.Should().Be(ResultStatus.Unauthorized); // Initial state 
        }
    }
}
