using System;
using System.Collections.Generic;

namespace Ardalis.Result.Sample.Core.Exceptions
{
    public class ForecastRequestInvalidException : Exception
    {
        public Dictionary<string,string> ValidationErrors { get; set; }

        public ForecastRequestInvalidException(Dictionary<string, string> validationErrors) : base("Forecast request is invalid.")
        {
            ValidationErrors = validationErrors;
        }

        public ForecastRequestInvalidException() : base("Forecast request is invalid.")
        {

        }
    }
}
