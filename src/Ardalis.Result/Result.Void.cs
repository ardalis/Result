using System.Collections.Generic;

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
            return new Result(ResultStatus.Error) { Errors = errorMessages };
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
            return new Result(ResultStatus.Error)
            {
                CorrelationId = correlationId,
                Errors = errorMessages
            };
        }

        /// <summary>
        /// Represents the validation error that prevents the underlying service from completing.
        /// </summary>
        /// <param name="validationError">The validation error encountered</param>
        /// <returns>A Result</returns>
        public new static Result Invalid(ValidationError validationError)
        {
            return new Result(ResultStatus.Invalid) { ValidationErrors = { validationError } };
        }

        /// <summary>
        /// Represents validation errors that prevent the underlying service from completing.
        /// </summary>
        /// <param name="validationErrors">A list of validation errors encountered</param>
        /// <returns>A Result</returns>
        public new static Result Invalid(params ValidationError[] validationErrors)
        {
            return new Result(ResultStatus.Invalid) { ValidationErrors = new List<ValidationError>(validationErrors) };
        }

        /// <summary>
        /// Represents validation errors that prevent the underlying service from completing.
        /// </summary>
        /// <param name="validationErrors">A list of validation errors encountered</param>
        /// <returns>A Result</returns>
        public new static Result Invalid(List<ValidationError> validationErrors)
        {
            return new Result(ResultStatus.Invalid) { ValidationErrors = validationErrors };
        }

        /// <summary>
        /// Represents the situation where a service was unable to find a requested resource.
        /// </summary>
        /// <returns>A Result</returns>
        public new static Result NotFound()
        {
            return new Result(ResultStatus.NotFound);
        }

        /// <summary>
        /// Represents the situation where a service was unable to find a requested resource.
        /// Error messages may be provided and will be exposed via the Errors property.
        /// </summary>
        /// <param name="errorMessages">A list of string error messages.</param>
        /// <returns>A Result</returns>
        public new static Result NotFound(params string[] errorMessages)
        {
            return new Result(ResultStatus.NotFound) { Errors = errorMessages };
        }

        /// <summary>
        /// The parameters to the call were correct, but the user does not have permission to perform some action.
        /// See also HTTP 403 Forbidden: https://en.wikipedia.org/wiki/List_of_HTTP_status_codes#4xx_client_errors
        /// </summary>
        /// <returns>A Result</returns>
        public new static Result Forbidden()
        {
            return new Result(ResultStatus.Forbidden);
        }

        /// <summary>
        /// This is similar to Forbidden, but should be used when the user has not authenticated or has attempted to authenticate but failed.
        /// See also HTTP 401 Unauthorized: https://en.wikipedia.org/wiki/List_of_HTTP_status_codes#4xx_client_errors
        /// </summary>
        /// <returns>A Result</returns>
        public new static Result Unauthorized()
        {
            return new Result(ResultStatus.Unauthorized);
        }
        
        /// <summary>
        /// Represents a situation where a service is in conflict due to the current state of a resource,
        /// such as an edit conflict between multiple concurrent updates.
        /// See also HTTP 409 Conflict: https://en.wikipedia.org/wiki/List_of_HTTP_status_codes#4xx_client_errors
        /// </summary>
        /// <returns>A Result<typeparamref name="T"/></returns>
        public new static Result Conflict()
        {
            return new Result(ResultStatus.Conflict);
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
            return new Result(ResultStatus.Conflict) { Errors = errorMessages };
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
            return new Result(ResultStatus.Unavailable) { Errors = errorMessages };
        }
        
        /// Represents a critical error that occurred during the execution of the service.
        /// Everything provided by the user was valid, but the service was unable to complete due to an exception.
        /// See also HTTP 500 Internal Server Error: https://en.wikipedia.org/wiki/List_of_HTTP_status_codes#5xx_server_errors
        /// </summary>
        /// <param name="errorMessages">A list of string error messages.</param>
        /// <returns>A Result</returns>
        public static Result CriticalError(params string[] errorMessages)
        {
            return new Result(ResultStatus.CriticalError) { Errors = errorMessages };
        }
    }
}
