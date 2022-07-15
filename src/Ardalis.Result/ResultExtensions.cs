using System;
using System.Linq;

namespace Ardalis.Result
{
    public static class ResultExtensions
    {
        public static Result<K> Map<T, K>(this Result<T> result, Func<T, K> func)
        {
            switch (result.Status)
            {
                case ResultStatus.Ok: return func(result);
                case ResultStatus.NotFound: return Result<K>.NotFound();
                case ResultStatus.Unauthorized: return Result<K>.Unauthorized();
                case ResultStatus.Forbidden: return Result<K>.Forbidden();
                case ResultStatus.Invalid: return Result<K>.Invalid(result.ValidationErrors);
                case ResultStatus.Error: return Result<K>.Error(result.Errors.ToArray());
                default:
                    throw new NotSupportedException($"Result {result.Status} conversion is not supported.");
            }
        }
    }
}
