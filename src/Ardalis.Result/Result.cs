using System;

namespace Ardalis.Result
{
    public class Result<T>
    {
        public Result(T value)
        {
            Value = value;
        }
        private Result(bool successful = false)
        {
            Successful = successful;
        }

        public T Value { get; }
        public bool Successful { get; } = true;

        public static Result<T> Unsuccessful()
        {
            return new Result<T>(false);
        }
    }
}
