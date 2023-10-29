using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Ardalis.Result
{
    public class Result<T> : IResult
    {
        protected Result() { }

        public Result(T value)
        {
            Value = value;
        }

        protected internal Result(T value, string successMessage) : this(value)
        {
            SuccessMessage = successMessage;
        }

        protected Result(ResultStatus status)
        {
            Status = status;
        }

        public static implicit operator T(Result<T> result) => result.Value;
        public static implicit operator Result<T>(T value) => new Result<T>(value);

        public static implicit operator Result<T>(Result result) => new Result<T>(default(T))
        {
            Status = result.Status,
            Errors = result.Errors,
            SuccessMessage = result.SuccessMessage,
            CorrelationId = result.CorrelationId,
            ValidationErrors = result.ValidationErrors,
        };

        public T Value { get; }

        [JsonIgnore]
        public Type ValueType => typeof(T);
        public ResultStatus Status { get; protected set; } = ResultStatus.Ok;
        public bool IsSuccess => Status == ResultStatus.Ok;
        public string SuccessMessage { get; protected set; } = string.Empty;
        public string CorrelationId { get; protected set; } = string.Empty;
        public IEnumerable<string> Errors { get; protected set; } = new List<string>();
        public List<ValidationError> ValidationErrors { get; protected set; } = new List<ValidationError>();

        /// <summary>
        /// Returns the current value.
        /// </summary>
        /// <returns></returns>
        public object GetValue()
        {
            return this.Value;
        }

        /// <summary>
        /// Converts PagedInfo into a PagedResult<typeparamref name="T"/>
        /// </summary>
        /// <param name="pagedInfo"></param>
        /// <returns></returns>
        public PagedResult<T> ToPagedResult(PagedInfo pagedInfo)
        {
            var pagedResult = new PagedResult<T>(pagedInfo, Value)
            {
                Status = Status,
                SuccessMessage = SuccessMessage,
                CorrelationId = CorrelationId,
                Errors = Errors,
                ValidationErrors = ValidationErrors
            };

            return pagedResult;
        }

        /// <summary>
        /// Represents a successful operation and accepts a values as the result of the operation
        /// </summary>
        /// <param name="value">Sets the Value property</param>
        /// <returns>A Result<typeparamref name="T"/></returns>
        public static Result<T> Success(T value)
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
        public static Result<T> Success(T value, string successMessage)
        {
            return new Result<T>(value, successMessage);
        }

        /// <summary>
        /// Represents an error that occurred during the execution of the service.
        /// Error messages may be provided and will be exposed via the Errors property.
        /// </summary>
        /// <param name="errorMessages">A list of string error messages.</param>
        /// <returns>A Result<typeparamref name="T"/></returns>
        public static Result<T> Error(params string[] errorMessages)
        {
            return new Result<T>(ResultStatus.Error) { Errors = errorMessages };
        }

        /// <summary>
        /// Represents a validation error that prevents the underlying service from completing.
        /// </summary>
        /// <param name="validationError">The validation error encountered</param>
        /// <returns>A Result<typeparamref name="T"/></returns>
        public static Result<T> Invalid(ValidationError validationError)
        {
            return new Result<T>(ResultStatus.Invalid) { ValidationErrors = { validationError } };
        }

        /// <summary>
        /// Represents validation errors that prevent the underlying service from completing.
        /// </summary>
        /// <param name="validationErrors">A list of validation errors encountered</param>
        /// <returns>A Result<typeparamref name="T"/></returns>
        public static Result<T> Invalid(params ValidationError[] validationErrors)
        {
            return new Result<T>(ResultStatus.Invalid) { ValidationErrors = new List<ValidationError>(validationErrors) };
        }

        /// <summary>
        /// Represents validation errors that prevent the underlying service from completing.
        /// </summary>
        /// <param name="validationErrors">A list of validation errors encountered</param>
        /// <returns>A Result<typeparamref name="T"/></returns>
        public static Result<T> Invalid(List<ValidationError> validationErrors)
        {
            return new Result<T>(ResultStatus.Invalid) { ValidationErrors = validationErrors };
        }

        /// <summary>
        /// Represents the situation where a service was unable to find a requested resource.
        /// </summary>
        /// <returns>A Result<typeparamref name="T"/></returns>
        public static Result<T> NotFound()
        {
            return new Result<T>(ResultStatus.NotFound);
        }

        /// <summary>
        /// Represents the situation where a service was unable to find a requested resource.
        /// Error messages may be provided and will be exposed via the Errors property.
        /// </summary>
        /// <param name="errorMessages">A list of string error messages.</param>
        /// <returns>A Result<typeparamref name="T"/></returns>
        public static Result<T> NotFound(params string[] errorMessages)
        {
            return new Result<T>(ResultStatus.NotFound) { Errors = errorMessages };
        }

        /// <summary>
        /// The parameters to the call were correct, but the user does not have permission to perform some action.
        /// See also HTTP 403 Forbidden: https://en.wikipedia.org/wiki/List_of_HTTP_status_codes#4xx_client_errors
        /// </summary>
        /// <returns>A Result<typeparamref name="T"/></returns>
        public static Result<T> Forbidden()
        {
            return new Result<T>(ResultStatus.Forbidden);
        }

        /// <summary>
        /// This is similar to Forbidden, but should be used when the user has not authenticated or has attempted to authenticate but failed.
        /// See also HTTP 401 Unauthorized: https://en.wikipedia.org/wiki/List_of_HTTP_status_codes#4xx_client_errors
        /// </summary>
        /// <returns>A Result<typeparamref name="T"/></returns>
        public static Result<T> Unauthorized()
        {
            return new Result<T>(ResultStatus.Unauthorized);
        }
        
        /// <summary>
        /// Represents a situation where a service is in conflict due to the current state of a resource,
        /// such as an edit conflict between multiple concurrent updates.
        /// See also HTTP 409 Conflict: https://en.wikipedia.org/wiki/List_of_HTTP_status_codes#4xx_client_errors
        /// </summary>
        /// <returns>A Result<typeparamref name="T"/></returns>
        public static Result<T> Conflict()
        {
            return new Result<T>(ResultStatus.Conflict);
        }
        
        /// <summary>
        /// Represents a situation where a service is in conflict due to the current state of a resource,
        /// such as an edit conflict between multiple concurrent updates.
        /// Error messages may be provided and will be exposed via the Errors property.
        /// See also HTTP 409 Conflict: https://en.wikipedia.org/wiki/List_of_HTTP_status_codes#4xx_client_errors
        /// </summary>
        /// <param name="errorMessages">A list of string error messages.</param>
        /// <returns>A Result<typeparamref name="T"/></returns>
        public static Result<T> Conflict(params string[] errorMessages)
        {
            return new Result<T>(ResultStatus.Conflict) { Errors = errorMessages };
        }
        
        /// <summary>
        /// Represents a critical error that occurred during the execution of the service.
        /// Everything provided by the user was valid, but the service was unable to complete due to an exception.
        /// See also HTTP 500 Internal Server Error: https://en.wikipedia.org/wiki/List_of_HTTP_status_codes#5xx_server_errors
        /// </summary>
        /// <param name="errorMessages">A list of string error messages.</param>
        /// <returns>A Result<typeparamref name="T"/></returns>
        public static Result<T> CriticalError(params string[] errorMessages)
        {
            return new Result<T>(ResultStatus.CriticalError) { Errors = errorMessages };
        }

        /// <summary>
        /// Represents a situation where a service is unavailable, such as when the underlying data store is unavailable.
        /// Errors may be transient, so the caller may wish to retry the operation.
        /// See also HTTP 503 Service Unavailable: https://en.wikipedia.org/wiki/List_of_HTTP_status_codes#5xx_server_errors
        /// </summary>
        /// <param name="errorMessages">A list of string error messages</param>
        /// <returns></returns>
        public static Result<T> Unavailable(params string[] errorMessages)
        {
            return new Result<T>(ResultStatus.Unavailable) { Errors = errorMessages};
        }
    }
}
