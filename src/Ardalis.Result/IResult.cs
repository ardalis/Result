using System;
using System.Collections.Generic;

namespace Ardalis.Result
{
    public interface IResult
    {
        ResultStatus Status { get; }
        IEnumerable<string> Errors { get; }
        Dictionary<string, string[]> ValidationErrors { get; }
        Type ValueType { get; }
        Object GetValue();
    }
}
