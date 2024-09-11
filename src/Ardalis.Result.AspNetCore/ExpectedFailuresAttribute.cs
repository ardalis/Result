using System;
using System.Collections.Generic;

namespace Ardalis.Result.AspNetCore
{
    [AttributeUsage(AttributeTargets.Method)]
    public class ExpectedFailuresAttribute(params ResultStatus[] resultStatuses) : Attribute
    {
        public IEnumerable<ResultStatus> ResultStatuses { get; } = resultStatuses;
    }
}
