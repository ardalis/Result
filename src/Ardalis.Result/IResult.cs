using System;
using System.Collections.Generic;

namespace Ardalis.Result
{
    public interface IResult<T>
    {
        ResultStatus Status { get; }
        IEnumerable<string> Errors { get; }
        List<ValidationError> ValidationErrors { get; }
        T Value { get; }
    }
}
