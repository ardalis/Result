using System;
using System.Collections.Generic;

namespace Ardalis.Result
{
    public interface IRequestResult
    {
        ResultStatus Status { get; }
        IEnumerable<string> Errors { get; }
        List<ValidationError> ValidationErrors { get; }
        Type ValueType { get; }
        Object GetValue();
    }
}
