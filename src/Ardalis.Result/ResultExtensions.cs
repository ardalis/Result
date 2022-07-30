using System;
using System.Linq;

namespace Ardalis.Result
{
    public static class ResultExtensions
    {
        /// <summary>
        /// Transforms a Result's type from a source type to a destination type. If the Result is successful, the func parameter is invoked on the Result's source value to map it to a destination type.
        /// </summary>
        /// <typeparam name="TSource"></typeparam>
        /// <typeparam name="TDestination"></typeparam>
        /// <param name="result"></param>
        /// <param name="func"></param>
        /// <returns></returns>
        /// <exception cref="NotSupportedException"></exception> 
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
