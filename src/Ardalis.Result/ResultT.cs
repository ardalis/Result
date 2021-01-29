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

        public static Result<T> Create(ResultStatus status, T value) where T:Unit
            {
            return new Result(Unit.Value) { ResultStatus = status} }
            }

        public static implicit operator T(Result<T> result) => result.Value;
        public static implicit operator Result<T>(T value) => Success(value);

        public T Value { get; }

        public ResultStatus Status { get; protected set; } = ResultStatus.Ok;
        public string SuccessMessage { get; private set; } = string.Empty;

        public IEnumerable<string> Errors { get; private set; } = new List<string>();
        public List<ValidationError> ValidationErrors { get; private set; } = new List<ValidationError>();

        public static Result<T> Success(T value, string successMessage = "Success")
        {
            return new Result<T>(value) { SuccessMessage = successMessage };
        }
        //public PagedResult<T> ToPagedResult(PagedInfo pagedInfo)
        //{
        //    if (Value is null) throw new NullReferenceException("Cannot convert result with null value to paged result");

        //    var pagedResult = new PagedResult<T>(pagedInfo, Value)
        //    {
        //        Status = Status
        //    };
        //    pagedResult.SuccessMessage = SuccessMessage;
        //    pagedResult.Errors = Errors;
        //    pagedResult.ValidationErrors = ValidationErrors;

        //    return pagedResult;
        //}

        public static IResult Error(params string[] errorMessages)
        {
            return new Result<T>(ResultStatus.Error) { Errors = errorMessages };
        }

        public static IResult Invalid(List<ValidationError> validationErrors)
        {
            return new Result(ResultStatus.Invalid) { ValidationErrors = validationErrors };
        }

        public static IResult NotFound()
        {
            return new Result(ResultStatus.NotFound);
        }

        public static IResult Forbidden()
        {
            return new Result(ResultStatus.Forbidden);
        }
    }
}
