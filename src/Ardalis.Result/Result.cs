using System;
using System.Collections.Generic;

namespace Ardalis.Result
{
    public class Result<T> : IResult
    {
        public Result(T value)
        {
            Value = value;
            if (Value != null)
            {
                ValueType = Value.GetType();
            }
        }

        private Result(ResultStatus status)
        {
            Status = status;
        }

        public static implicit operator T(Result<T> result) => result.Value;
        public static implicit operator Result<T>(T value) => Success(value);

        public T Value { get; }

        public Type ValueType { get; private set; }
        public ResultStatus Status { get; private set; } = ResultStatus.Ok;
        public bool IsSuccess => Status == ResultStatus.Ok;
        public string SuccessMessage { get; private set; } = string.Empty;
        public IEnumerable<string> Errors { get; private set; } = new List<string>();
        public List<ValidationError> ValidationErrors { get; private set; } = new List<ValidationError>();

        public void ClearValueType() => ValueType = null;

        public object GetValue()
        {
            return this.Value;
        }

        public PagedResult<T> ToPagedResult(PagedInfo pagedInfo)
        {
            var pagedResult = new PagedResult<T>(pagedInfo, Value)
            {
                Status = Status, 
                SuccessMessage = SuccessMessage,
                Errors = Errors,
                ValidationErrors = ValidationErrors
            };

            return pagedResult;
        }

        public static Result<T> Success(T value)
        {
            return new Result<T>(value);
        }

        public static Result<T> Success(T value, string successMessage)
        {
            return new Result<T>(value) {SuccessMessage = successMessage};
        }

        public static Result<T> Error(params string[] errorMessages)
        {
            return new Result<T>(ResultStatus.Error) { Errors = errorMessages };
        }

        public static Result<T> Invalid(List<ValidationError> validationErrors)
        {
            return new Result<T>(ResultStatus.Invalid) { ValidationErrors = validationErrors };
        }

        public static Result<T> NotFound()
        {
            return new Result<T>(ResultStatus.NotFound);
        }

        public static Result<T> Forbidden()
        {
            return new Result<T>(ResultStatus.Forbidden);
        }
    }
}
