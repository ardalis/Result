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
                case ResultStatus.Created: return string.IsNullOrEmpty(result.Location)
                        ? Result<TDestination>.Created(func(result.Value))
                        : Result<TDestination>.Created(func(result.Value), result.Location);
                case ResultStatus.NotFound: return result.Errors.Any()
                        ? Result<TDestination>.NotFound(result.Errors.ToArray())
                        : Result<TDestination>.NotFound();
                case ResultStatus.Unauthorized: return result.Errors.Any()
                                        ? Result<TDestination>.Unauthorized(result.Errors.ToArray())
                                        : Result<TDestination>.Unauthorized();
                case ResultStatus.Forbidden: return result.Errors.Any()
                                        ? Result<TDestination>.Forbidden(result.Errors.ToArray())
                                        : Result<TDestination>.Forbidden();
                case ResultStatus.Invalid: return Result<TDestination>.Invalid(result.ValidationErrors);
                case ResultStatus.Error: return Result<TDestination>.Error(new ErrorList(result.Errors.ToArray(), result.CorrelationId));
                case ResultStatus.Conflict: return result.Errors.Any()
                                        ? Result<TDestination>.Conflict(result.Errors.ToArray())
                                        : Result<TDestination>.Conflict();
                case ResultStatus.CriticalError: return Result<TDestination>.CriticalError(result.Errors.ToArray());
                case ResultStatus.Unavailable: return Result<TDestination>.Unavailable(result.Errors.ToArray());
                case ResultStatus.NoContent: return Result<TDestination>.NoContent();
                default:
                    throw new NotSupportedException($"Result {result.Status} conversion is not supported.");
            }
        }
        
        /// <summary>
        /// Transforms a Result's type from a source type to a non-generic Result type.
        /// </summary>
        /// <typeparam name="TSource"></typeparam>
        /// <param name="result"></param>
        /// <returns></returns>
        /// <exception cref="NotSupportedException"></exception> 
        public static Result Map<TSource>(this Result<TSource> result)
        {
            switch (result.Status)
            {
                case ResultStatus.Ok: return Result.Success();
                case ResultStatus.NotFound: return result.Errors.Any()
                    ? Result.NotFound(result.Errors.ToArray())
                    : Result.NotFound();
                case ResultStatus.Unauthorized: return result.Errors.Any()
                    ? Result.Unauthorized(result.Errors.ToArray())
                    : Result.Unauthorized();
                case ResultStatus.Forbidden: return result.Errors.Any()
                    ? Result.Forbidden(result.Errors.ToArray())
                    : Result.Forbidden();
                case ResultStatus.Invalid: return Result.Invalid(result.ValidationErrors);
                case ResultStatus.Error: return Result.Error(new ErrorList(result.Errors.ToArray(), result.CorrelationId));
                case ResultStatus.Conflict: return result.Errors.Any()
                    ? Result.Conflict(result.Errors.ToArray())
                    : Result.Conflict();
                case ResultStatus.CriticalError: return Result.CriticalError(result.Errors.ToArray());
                case ResultStatus.Unavailable: return Result.Unavailable(result.Errors.ToArray());
                case ResultStatus.NoContent: return Result.NoContent();
                default:
                    throw new NotSupportedException($"Result {result.Status} conversion is not supported.");
            }
        }
    }
}
