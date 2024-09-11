using System;
using System.Collections.Generic;

namespace Ardalis.Result.AspNetCore.Exceptions
{
    internal class UnexpectedFailureResultsException(IEnumerable<ResultStatus> statuses) : Exception
    {
        public IEnumerable<ResultStatus> UnexpectedStatuses { get; } = statuses;

        public override string ToString()
            => $"ActionModel has [{nameof(ExpectedFailuresAttribute)}] with result statuses which are not configured in ResultConvention.";
    }
}
