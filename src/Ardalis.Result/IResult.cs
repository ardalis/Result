using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Ardalis.Result
{
    public interface IResult
    {
        ResultStatus Status { get; }
        ObservableCollection<string> Errors { get; }
        ObservableCollection<ValidationError> ValidationErrors { get; }
        Type ValueType { get; }
        Object GetValue();
    }
}
