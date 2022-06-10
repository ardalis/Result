using System;
using System.Collections.Generic;

namespace Ardalis.Result.AspNetCore
{
    public class ExpectedFailureResultStatusesAttribute : Attribute
    {
        public ExpectedFailureResultStatusesAttribute(params ResultStatus[] resultStatuses)
        {
            ResultStatuses = resultStatuses;
        }

        public IEnumerable<ResultStatus> ResultStatuses { get; }
    }
}
