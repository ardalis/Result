using System;
using System.Collections.Generic;

namespace Ardalis.Result
{
    public class Result<T>
    {
        public Result(T value)
        {
            Value = value;
        }
        private Result(ResultStatus status)
        {
            Status = status;
        }

        public T Value { get; }
        public ResultStatus Status { get; } = ResultStatus.Ok;
        public IEnumerable<string> Errors { get; private set; } = new List<string>();
        public Dictionary<string, string> ValidationErrors { get; private set;} = new Dictionary<string, string>();

        public static Result<T> Success(T value)
        {
            return new Result<T>(value);
        }

        public static Result<T> Error(params string[] errorMessages)
        {
            return new Result<T>(ResultStatus.Error) { Errors = errorMessages };
        }

        public static Result<T> Invalid(Dictionary<string, string> validationErrors)
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
