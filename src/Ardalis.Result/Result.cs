using System.Collections.Generic;

namespace Ardalis.Result
{
    public class Result : IResult
    {
        internal Result(ResultStatus status)
        {
            Status = status;
        }

        public ResultStatus Status { get; private set; } = ResultStatus.Ok;
        public string? SuccessMessage { get; private set; }
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

        public PagedResult ToPagedResult(PagedInfo pagedInfo)
        {
            var pagedResult = new PagedResult(pagedInfo, Status)
            {
                SuccessMessage = SuccessMessage,
                Errors = Errors,
                ValidationErrors = ValidationErrors
            };

            return pagedResult;
        }

    }
}
