using System;

namespace Ardalis.Result.Sample.Core.Exceptions
{
    public class ForecastNotFoundException : Exception
    {
        public ForecastNotFoundException() : base("Forecast not found.") { }
    }
}
