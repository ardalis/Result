using System.Collections.Generic;

namespace Ardalis.Result
{
    public interface IResult
    {
        ResultStatus Status { get; }
        IEnumerable<string> Errors { get; }
        List<ValidationError> ValidationErrors { get; }
    }
}
