using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace Ardalis.Result
{
    public class Result : Result<Result>
    {
        public Result() : base() { }

        protected internal Result(ResultStatus status) : base(status) { }

        /// <summary>
        /// Represents a successful operation without return type
        /// </summary>
        /// <returns>A Result</returns>
        public static Result Success()
        {
            return new Result();
        }

        /// <summary>
        /// Represents a successful operation without return type
        /// </summary>
        /// <param name="successMessage">Sets the SuccessMessage property</param>
        /// <returns>A Result></returns>
        public static Result SuccessWithMessage(string successMessage)
        {
            return new Result() { SuccessMessage = successMessage };
        }

        /// <summary>
        /// Represents a successful operation and accepts a values as the result of the operation
        /// </summary>
        /// <param name="value">Sets the Value property</param>
        /// <returns>A Result<typeparamref name="T"/></returns>
        public static Result<T> Success<T>(T value)
        {
            return new Result<T>(value);
        }

        /// <summary>
        /// Represents a successful operation and accepts a values as the result of the operation
        /// Sets the SuccessMessage property to the provided value
        /// </summary>
        /// <param name="value">Sets the Value property</param>
        /// <param name="successMessage">Sets the SuccessMessage property</param>
        /// <returns>A Result<typeparamref name="T"/></returns>
        public static Result<T> Success<T>(T value, string successMessage)
        {
            return new Result<T>(value, successMessage);
        }

        /// <summary>
        /// Represents an error that occurred during the execution of the service.
        /// Error messages may be provided and will be exposed via the Errors property.
        /// </summary>
        /// <param name="errorMessages">A list of string error messages.</param>
        /// <returns>A Result</returns>
        public new static Result Error(params string[] errorMessages)
        {
            Result result = new Result(ResultStatus.Error);

            if (errorMessages != null && errorMessages.Length > 0)
                result.Errors = new ObservableCollection<string>(errorMessages);

            result.Initialize();

            return result;
        }

        /// <summary>
        /// Represents an error that occurred during the execution of the service.
        /// Sets the CorrelationId property to the provided value
        /// Error messages may be provided and will be exposed via the Errors property.
        /// </summary>
        /// <param name="correlationId">Sets the CorrelationId property.</param>
        /// <param name="errorMessages">A list of string error messages.</param>
        /// <returns>A Result</returns>
        public static Result ErrorWithCorrelationId(string correlationId, params string[] errorMessages)
        {
            Result result = new Result(ResultStatus.Error);
            result.CorrelationId = correlationId;

            if (errorMessages != null && errorMessages.Length > 0)
                result.Errors = new ObservableCollection<string>(errorMessages);

            result.Initialize();

            return result;
        }

        /// <summary>
        /// Represents the validation error that prevents the underlying service from completing.
        /// </summary>
        /// <param name="validationError">The validation error encountered</param>
        /// <returns>A Result</returns>
        public new static Result Invalid(ValidationError validationError)
        {
            return Invalid(new List<ValidationError> { validationError });
        }

        /// <summary>
        /// Represents validation errors that prevent the underlying service from completing.
        /// </summary>
        /// <param name="validationErrors">A list of validation errors encountered</param>
        /// <returns>A Result</returns>
        public new static Result Invalid(params ValidationError[] validationErrors)
        {
            Result result = new Result(ResultStatus.Invalid);

            if (validationErrors != null && validationErrors.Length > 0)
                result.ValidationErrors = new ObservableCollection<ValidationError>(validationErrors);

            result.Initialize();

            return result;
        }

        /// <summary>
        /// Represents validation errors that prevent the underlying service from completing.
        /// </summary>
        /// <param name="validationErrors">A list of validation errors encountered</param>
        /// <returns>A Result</returns>
        public new static Result Invalid(List<ValidationError> validationErrors)
        {
            return Invalid(validationErrors.ToArray());
        }

        /// <summary>
        /// Represents the situation where a service was unable to find a requested resource.
        /// </summary>
        /// <returns>A Result</returns>
        public new static Result NotFound()
        {
            return NotFound(null);
        }

        /// <summary>
        /// Represents the situation where a service was unable to find a requested resource.
        /// Error messages may be provided and will be exposed via the Errors property.
        /// </summary>
        /// <param name="errorMessages">A list of string error messages.</param>
        /// <returns>A Result</returns>
        public new static Result NotFound(params string[] errorMessages)
        {
            Result result = new Result(ResultStatus.NotFound);

            if (errorMessages != null && errorMessages.Length > 0)
                result.Errors = new ObservableCollection<string>(errorMessages);

            result.Initialize();

            return result;
        }

        /// <summary>
        /// The parameters to the call were correct, but the user does not have permission to perform some action.
        /// See also HTTP 403 Forbidden: https://en.wikipedia.org/wiki/List_of_HTTP_status_codes#4xx_client_errors
        /// </summary>
        /// <returns>A Result</returns>
        public new static Result Forbidden()
        {
            Result result = new Result(ResultStatus.Forbidden);
            result.Initialize();
            return result;
        }

        /// <summary>
        /// This is similar to Forbidden, but should be used when the user has not authenticated or has attempted to authenticate but failed.
        /// See also HTTP 401 Unauthorized: https://en.wikipedia.org/wiki/List_of_HTTP_status_codes#4xx_client_errors
        /// </summary>
        /// <returns>A Result</returns>
        public new static Result Unauthorized()
        {
            Result result = new Result(ResultStatus.Unauthorized);
            result.Initialize();
            return result;
        }

        /// <summary>
        /// Represents a situation where a service is in conflict due to the current state of a resource,
        /// such as an edit conflict between multiple concurrent updates.
        /// See also HTTP 409 Conflict: https://en.wikipedia.org/wiki/List_of_HTTP_status_codes#4xx_client_errors
        /// </summary>
        /// <returns>A Result<typeparamref name="T"/></returns>
        public new static Result Conflict()
        {
            return Conflict(null);
        }

        /// <summary>
        /// Represents a situation where a service is in conflict due to the current state of a resource,
        /// such as an edit conflict between multiple concurrent updates.
        /// Error messages may be provided and will be exposed via the Errors property.
        /// See also HTTP 409 Conflict: https://en.wikipedia.org/wiki/List_of_HTTP_status_codes#4xx_client_errors
        /// </summary>
        /// <param name="errorMessages">A list of string error messages.</param>
        /// <returns>A Result<typeparamref name="T"/></returns>
        public new static Result Conflict(params string[] errorMessages)
        {
            Result result = new Result(ResultStatus.Conflict);

            if (errorMessages != null && errorMessages.Length > 0)
                result.Errors = new ObservableCollection<string>(errorMessages);

            result.Initialize();

            return result;
        }

        /// <summary>
        /// Represents a situation where a service is unavailable, such as when the underlying data store is unavailable.
        /// Errors may be transient, so the caller may wish to retry the operation.
        /// See also HTTP 503 Service Unavailable: https://en.wikipedia.org/wiki/List_of_HTTP_status_codes#5xx_server_errors
        /// </summary>
        /// <param name="errorMessages">A list of string error messages</param>
        /// <returns></returns>
        public new static Result Unavailable(params string[] errorMessages)
        {
            Result result = new Result(ResultStatus.Unavailable);

            if (errorMessages != null && errorMessages.Length > 0)
                result.Errors = new ObservableCollection<string>(errorMessages);

            result.Initialize();

            return result;
        }

        /// Represents a critical error that occurred during the execution of the service.
        /// Everything provided by the user was valid, but the service was unable to complete due to an exception.
        /// See also HTTP 500 Internal Server Error: https://en.wikipedia.org/wiki/List_of_HTTP_status_codes#5xx_server_errors
        /// </summary>
        /// <param name="errorMessages">A list of string error messages.</param>
        /// <returns>A Result</returns>
        public static Result CriticalError(params string[] errorMessages)
        {
            Result result = new Result(ResultStatus.CriticalError);

            if (errorMessages != null && errorMessages.Length > 0)
                result.Errors = new ObservableCollection<string>(errorMessages);

            result.Initialize();

            return result;
        }
    }
}
