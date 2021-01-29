using System;
using System.Collections.Generic;

namespace Ardalis.Result
{
    public class Result<T> : IResult<T>
    {
        public Result(T value)
        {
            if (value == null) throw new ArgumentNullException(nameof(value), "value is required; use non-generic Result if no value needed.");
            Value = value;
        }
        private Result(ResultStatus status)
        {
            Status = status;
        }

        public static implicit operator T(Result<T> result) => result.Value;
        public static implicit operator Result<T>(T value) => Success(value);

        public T Value { get; }

        public ResultStatus Status { get; } = ResultStatus.Ok;
        public IEnumerable<string> Errors { get; private set; } = new List<string>();
        public List<ValidationError> ValidationErrors { get; private set; } = new List<ValidationError>();

        public static Result<T> Success(T value)
        {
            return new Result<T>(value);
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
