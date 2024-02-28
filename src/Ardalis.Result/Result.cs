using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Text.Json.Serialization;

namespace Ardalis.Result
{
    public class Result<T> : IResult
    {
        protected Result()
        {
            Initialize();
        }

        public Result(T value)
        {
            Value = value;
            Initialize();
        }

        protected internal Result(T value, string successMessage) : this(value)
        {
            SuccessMessage = successMessage;
            Initialize();
        }

        protected Result(ResultStatus status)
        {
            Status = status;
            Initialize();
        }

        protected void Initialize()
        {
            InitialStatus = Status;
            ValidationErrors.CollectionChanged += ValidationErrors_CollectionChanged;
            Errors.CollectionChanged += Errors_CollectionChanged;
        }

        protected void ValidationErrors_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            EvaluateResultStatus();
        }
        protected void Errors_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            EvaluateResultStatus();
        }

        private void EvaluateResultStatus()
        {
            if (ShouldParseStatusBasedOnResultStatus())
            {
                if (Errors.Count > 0)
                    Status = ResultStatus.Error;
                else if (ValidationErrors.Count > 0)
                    Status = ResultStatus.Invalid;
                else
                    Status = InitialStatus;
            }
        }

        private bool ShouldParseStatusBasedOnResultStatus()
        {
            return Status == ResultStatus.Ok || 
                   Status == ResultStatus.Error || 
                   Status == ResultStatus.Invalid;
        }

        public static implicit operator T(Result<T> result) => result.Value;
        public static implicit operator Result<T>(T value) => new Result<T>(value);

        public static implicit operator Result<T>(Result result) => new Result<T>(default(T))
        {
            Status = result.Status,
            InitialStatus = result.InitialStatus,
            Errors = result.Errors,
            SuccessMessage = result.SuccessMessage,
            CorrelationId = result.CorrelationId,
            ValidationErrors = result.ValidationErrors,
        };

        [JsonInclude] 
        public T Value { get; init; }

        [JsonIgnore]
        public Type ValueType => typeof(T);

        // Saves the initial status used by the static constructors. Used to revert back to this status when the user changes the Errors or ValidationErrors collections.
        private ResultStatus InitialStatus { get; set; } = ResultStatus.Ok;

        [JsonInclude] 
        public ResultStatus Status { get; protected set; } = ResultStatus.Ok;
        public bool IsSuccess => Status == ResultStatus.Ok;
        [JsonInclude] 
        public string SuccessMessage { get; protected set; } = string.Empty;
        [JsonInclude] 
        public string CorrelationId { get; protected set; } = string.Empty;

        private ObservableCollection<string> _errors = new ObservableCollection<string>();
        [JsonInclude] 
        public ObservableCollection<string> Errors
        {
            get
            {
                return _errors;
            }
            set
            {
                _errors = value;
                Errors_CollectionChanged(_errors, new System.Collections.Specialized.NotifyCollectionChangedEventArgs(System.Collections.Specialized.NotifyCollectionChangedAction.Reset));
            }
        }

        private ObservableCollection<ValidationError> _validationErrors = new ObservableCollection<ValidationError>();
        [JsonInclude] 
        public ObservableCollection<ValidationError> ValidationErrors
        {
            get
            {
                return _validationErrors;
            }
            set
            {
                _validationErrors = value;
                ValidationErrors_CollectionChanged(_validationErrors, new System.Collections.Specialized.NotifyCollectionChangedEventArgs(System.Collections.Specialized.NotifyCollectionChangedAction.Reset));
            }
        }

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
                InitialStatus = InitialStatus,
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
            Result<T> result = new Result<T>(ResultStatus.Error);

            if (errorMessages != null && errorMessages.Length > 0)
                result.Errors = new ObservableCollection<string>(errorMessages);

            return result;
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
            Result<T> result = new Result<T>(ResultStatus.Invalid);

            if (validationErrors != null && validationErrors.Length > 0)
                result.ValidationErrors = new ObservableCollection<ValidationError>(validationErrors);

            return result;
        }

        /// <summary>
        /// Represents validation errors that prevent the underlying service from completing.
        /// </summary>
        /// <param name="validationErrors">A list of validation errors encountered</param>
        /// <returns>A Result<typeparamref name="T"/></returns>
        public static Result<T> Invalid(List<ValidationError> validationErrors)
        {
            return Invalid(validationErrors.ToArray());
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
            Result<T> result = new Result<T>(ResultStatus.NotFound);

            if (errorMessages != null || errorMessages.Length > 0)
                result.Errors = new ObservableCollection<string>(errorMessages);

            return result;
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
            Result<T> result = new Result<T>(ResultStatus.Conflict);

            if (errorMessages != null || errorMessages.Length > 0)
                result.Errors = new ObservableCollection<string>(errorMessages);

            return result;
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
            Result<T> result = new Result<T>(ResultStatus.CriticalError);

            if (errorMessages != null || errorMessages.Length > 0)
                result.Errors = new ObservableCollection<string>(errorMessages);

            return result;
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
            Result<T> result = new Result<T>(ResultStatus.Unavailable);

            if (errorMessages != null || errorMessages.Length > 0)
                result.Errors = new ObservableCollection<string>(errorMessages);

            return result;
        }
    }
}
