using System;
using System.Collections.Generic;

namespace Ardalis.Result.AspNetCore
{
    public class ExpectedFailuresAttribute : Attribute
    {
        public ExpectedFailuresAttribute(params ResultStatus[] resultStatuses)
        {
            ResultStatuses = resultStatuses;
        }

        public IEnumerable<ResultStatus> ResultStatuses { get; }
    }
}
