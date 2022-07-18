using System;
using System.Linq;

namespace Ardalis.Result
{
    public static class ResultExtensions
    {
        public static Result<TDestination> Map<TSource, TDestination>(this Result<TSource> result, Func<TSource, TDestination> func)
        {
            switch (result.Status)
            {
                case ResultStatus.Ok: return func(result);
                case ResultStatus.NotFound: return Result<TDestination>.NotFound();
                case ResultStatus.Unauthorized: return Result<TDestination>.Unauthorized();
                case ResultStatus.Forbidden: return Result<TDestination>.Forbidden();
                case ResultStatus.Invalid: return Result<TDestination>.Invalid(result.ValidationErrors);
                case ResultStatus.Error: return Result<TDestination>.Error(result.Errors.ToArray());
                default:
                    throw new NotSupportedException($"Result {result.Status} conversion is not supported.");
            }
        }
    }
}
