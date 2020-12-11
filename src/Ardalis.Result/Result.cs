using System.Collections.Generic;

namespace Ardalis.Result
{
    public class Result : IResult
    {
        private Result(ResultStatus status)
        {
            Status = status;
        }

        public ResultStatus Status { get; } = ResultStatus.Ok;
        public IEnumerable<string> Errors { get; private set; } = new List<string>();
        public List<ValidationError> ValidationErrors { get; private set; } = new List<ValidationError>();

        public static Result Success()
        {
            return new Result(ResultStatus.Ok);
        }

        public static Result Error(params string[] errorMessages)
        {
            return new Result(ResultStatus.Error) { Errors = errorMessages };
        }

        public static Result Invalid(List<ValidationError> validationErrors)
        {
            return new Result(ResultStatus.Invalid) { ValidationErrors = validationErrors };
        }

        public static Result NotFound()
        {
            return new Result(ResultStatus.NotFound);
        }

        public static Result Forbidden()
        {
            return new Result(ResultStatus.Forbidden);
        }

    }
}
