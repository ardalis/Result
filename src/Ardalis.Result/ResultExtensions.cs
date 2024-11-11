using System;
using System.Linq;
using System.Threading.Tasks;

namespace Ardalis.Result
{
    public static class ResultExtensions
    {
        /// <summary>
        /// Transforms a Result's type from a source type to a destination type.
        /// If the Result is successful, the func parameter is invoked on the Result's source value to map it to a destination type.
        /// </summary>
        /// <typeparam name="TSource">The type of the value contained in the source Result.</typeparam>
        /// <typeparam name="TDestination">The type of the value to be returned in the destination Result.</typeparam>
        /// <param name="result">The source Result to transform.</param>
        /// <param name="func">A function to transform the source value to the destination type.</param>
        /// <returns>A Result containing the transformed value or the appropriate error status.</returns>
        /// <exception cref="NotSupportedException">Thrown when the Result status is not supported.</exception>
        public static Result<TDestination> Map<TSource, TDestination>(
            this Result<TSource> result,
            Func<TSource, TDestination> func
        )
        {
            return result.Status switch
            {
                ResultStatus.Ok => (Result<TDestination>)func(result),
                ResultStatus.Created => string.IsNullOrEmpty(result.Location)
                    ? Result<TDestination>.Created(func(result.Value))
                    : Result<TDestination>.Created(func(result.Value), result.Location),
                _ => HandleNonSuccessStatus<TSource, TDestination>(result),
            };
        }

        public static Result<TDestination> Map<TDestination>(
            this Result result,
            Func<TDestination> func
        )
        {
            return result.Status switch
            {
                ResultStatus.Ok => Result<TDestination>.Success(func()),
                ResultStatus.Created => string.IsNullOrEmpty(result.Location)
                    ? Result<TDestination>.Created(func())
                    : Result<TDestination>.Created(func(), result.Location),
                _ => HandleNonSuccessStatus<TDestination>(result),
            };
        }

        public static async Task<Result<TDestination>> MapAsync<TSource, TDestination>(
            this Result<TSource> result,
            Func<TSource, Task<TDestination>> func
        )
        {
            return result.Status switch
            {
                ResultStatus.Ok => Result<TDestination>.Success(await func(result.Value)),
                ResultStatus.Created => string.IsNullOrEmpty(result.Location)
                    ? Result<TDestination>.Created(await func(result.Value))
                    : Result<TDestination>.Created(await func(result.Value), result.Location),
                _ => HandleNonSuccessStatus<TSource, TDestination>(result),
            };
        }

        public static async Task<Result<TDestination>> MapAsync<TSource, TDestination>(
            this Task<Result<TSource>> resultTask,
            Func<TSource, Task<TDestination>> func
        )
        {
            var result = await resultTask;
            return result.Status switch
            {
                ResultStatus.Ok => Result<TDestination>.Success(await func(result.Value)),
                ResultStatus.Created => string.IsNullOrEmpty(result.Location)
                    ? Result<TDestination>.Created(await func(result.Value))
                    : Result<TDestination>.Created(await func(result.Value), result.Location),
                _ => HandleNonSuccessStatus<TSource, TDestination>(result),
            };
        }

        public static async Task<Result<TDestination>> MapAsync<TDestination>(
            this Result result,
            Func<Task<TDestination>> func
        )
        {
            return result.Status switch
            {
                ResultStatus.Ok => Result<TDestination>.Success(await func()),
                ResultStatus.Created => string.IsNullOrEmpty(result.Location)
                    ? Result<TDestination>.Created(await func())
                    : Result<TDestination>.Created(await func(), result.Location),
                _ => HandleNonSuccessStatus<TDestination>(result),
            };
        }

        public static async Task<Result<TDestination>> MapAsync<TDestination>(
            this Task<Result> resultTask,
            Func<Task<TDestination>> func
        )
        {
            var result = await resultTask;
            return await result.MapAsync(func);
        }

        public static async Task<Result<TDestination>> MapAsync<TSource, TDestination>(
            this Task<Result<TSource>> resultTask,
            Func<TSource, TDestination> func
        )
        {
            var result = await resultTask;
            return result.Map(func);
        }

        public static async Task<Result<TDestination>> MapAsync<TDestination>(
            this Task<Result> resultTask,
            Func<TDestination> func
        )
        {
            var result = await resultTask;
            return result.Map(func);
        }

        /// <summary>
        /// Transforms a Result's type from a source type to a destination type.
        /// If the Result is successful, the func parameter is invoked on the Result's source value to map it to a destination type.
        /// </summary>
        /// <typeparam name="TSource">The type of the value contained in the source Result.</typeparam>
        /// <typeparam name="TDestination">The type of the value to be returned in the destination Result.</typeparam>
        /// <param name="result">The source Result to transform.</param>
        /// <param name="func">A function to transform the source value to the destination type.</param>
        /// <returns>A Result containing the transformed value or the appropriate error status.</returns>
        /// <exception cref="NotSupportedException">Thrown when the Result status is not supported.</exception>
        public static Result<TDestination> Bind<TSource, TDestination>(
            this Result<TSource> result,
            Func<TSource, Result<TDestination>> bindFunc
        )
        {
            return result.Status switch
            {
                ResultStatus.Ok => bindFunc(result.Value),
                ResultStatus.Created => bindFunc(result.Value),
                _ => HandleNonSuccessStatus<TSource, TDestination>(result),
            };
        }

        public static Result<TDestination> Bind<TDestination>(
            this Result result,
            Func<Result, Result<TDestination>> bindFunc
        )
        {
            return result.Status switch
            {
                ResultStatus.Ok => bindFunc(result.Value),
                ResultStatus.Created => bindFunc(result.Value),
                _ => HandleNonSuccessStatus<TDestination>(result),
            };
        }

        public static Result Bind<TSource>(
            this Result<TSource> result,
            Func<TSource, Result> bindFunc
        )
        {
            return result.Status switch
            {
                ResultStatus.Ok => bindFunc(result.Value),
                ResultStatus.Created => bindFunc(result.Value),
                _ => HandleNonSuccessStatus(result),
            };
        }

        public static async Task<Result<TDestination>> BindAsync<TSource, TDestination>(
            this Task<Result<TSource>> resultTask,
            Func<TSource, Task<Result<TDestination>>> bindFunc
        )
        {
            var result = await resultTask;
            return result.Status switch
            {
                ResultStatus.Ok => await bindFunc(result.Value).ConfigureAwait(false),
                ResultStatus.Created => await bindFunc(result.Value).ConfigureAwait(false),
                _ => HandleNonSuccessStatus<TSource, TDestination>(result),
            };
        }

        public static async Task<Result> BindAsync<TSource>(
            this Task<Result<TSource>> resultTask,
            Func<TSource, Task<Result>> bindFunc
        )
        {
            var result = await resultTask;
            return result.Status switch
            {
                ResultStatus.Ok => await bindFunc(result.Value).ConfigureAwait(false),
                ResultStatus.Created => await bindFunc(result.Value).ConfigureAwait(false),
                _ => HandleNonSuccessStatus(result),
            };
        }

        public static async Task<Result> BindAsync<TSource>(
            this Result<TSource> result,
            Func<TSource, Task<Result>> bindFunc
        )
        {
            return result.Status switch
            {
                ResultStatus.Ok => await bindFunc(result.Value).ConfigureAwait(false),
                ResultStatus.Created => await bindFunc(result.Value).ConfigureAwait(false),
                _ => HandleNonSuccessStatus(result),
            };
        }

        public static async Task<Result> BindAsync(
            this Result result,
            Func<Result, Task<Result>> bindFunc
        )
        {
            return result.Status switch
            {
                ResultStatus.Ok => await bindFunc(result.Value).ConfigureAwait(false),
                ResultStatus.Created => await bindFunc(result.Value).ConfigureAwait(false),
                _ => HandleNonSuccessStatus(result),
            };
        }

        public static async Task<Result<TDestination>> BindAsync<TDestination>(
            this Task<Result> resultTask,
            Func<Result, Task<Result<TDestination>>> bindFunc
        )
        {
            var result = await resultTask;
            return result.Status switch
            {
                ResultStatus.Ok => await bindFunc(result.Value).ConfigureAwait(false),
                ResultStatus.Created => await bindFunc(result.Value).ConfigureAwait(false),
                _ => HandleNonSuccessStatus<TDestination>(result),
            };
        }

        public static async Task<Result<TDestination>> BindAsync<TSource, TDestination>(
            this Result<TSource> result,
            Func<TSource, Task<Result<TDestination>>> bindFunc
        )
        {
            return result.Status switch
            {
                ResultStatus.Ok => await bindFunc(result.Value).ConfigureAwait(false),
                ResultStatus.Created => await bindFunc(result.Value).ConfigureAwait(false),
                _ => HandleNonSuccessStatus<TSource, TDestination>(result),
            };
        }

        public static async Task<Result> BindAsync<TSource, TDestination>(
            this Result<TSource> result,
            Func<TSource, Task<Result>> bindFunc
        )
        {
            return result.Status switch
            {
                ResultStatus.Ok => await bindFunc(result.Value).ConfigureAwait(false),
                ResultStatus.Created => await bindFunc(result.Value).ConfigureAwait(false),
                _ => HandleNonSuccessStatus(result),
            };
        }

        public static async Task<Result> BindAsync(
            this Task<Result> resultTask,
            Func<Result, Task<Result>> bindFunc
        )
        {
            var result = await resultTask;
            return result.Status switch
            {
                ResultStatus.Ok => await bindFunc(result).ConfigureAwait(false),
                ResultStatus.Created => await bindFunc(result).ConfigureAwait(false),
                _ => HandleNonSuccessStatus(result),
            };
        }

        public static async Task<Result<TDestination>> BindAsync<TSource, TDestination>(
            this Task<Result<TSource>> resultTask,
            Func<TSource, Result<TDestination>> bindFunc
        )
        {
            var result = await resultTask;
            return result.Status switch
            {
                ResultStatus.Ok => bindFunc(result.Value),
                ResultStatus.Created => bindFunc(result.Value),
                _ => HandleNonSuccessStatus<TSource, TDestination>(result),
            };
        }

        private static Result<TDestination> HandleNonSuccessStatus<TSource, TDestination>(
            Result<TSource> result
        )
        {
            return result.Status switch
            {
                ResultStatus.NotFound => result.Errors.Any()
                    ? Result<TDestination>.NotFound(result.Errors.ToArray())
                    : Result<TDestination>.NotFound(),
                ResultStatus.Unauthorized => result.Errors.Any()
                    ? Result<TDestination>.Unauthorized(result.Errors.ToArray())
                    : Result<TDestination>.Unauthorized(),
                ResultStatus.Forbidden => result.Errors.Any()
                    ? Result<TDestination>.Forbidden(result.Errors.ToArray())
                    : Result<TDestination>.Forbidden(),
                ResultStatus.Invalid => Result<TDestination>.Invalid(result.ValidationErrors),
                ResultStatus.Error => Result<TDestination>.Error(
                    new ErrorList(result.Errors.ToArray(), result.CorrelationId)
                ),
                ResultStatus.Conflict => result.Errors.Any()
                    ? Result<TDestination>.Conflict(result.Errors.ToArray())
                    : Result<TDestination>.Conflict(),
                ResultStatus.CriticalError => Result<TDestination>.CriticalError(
                    result.Errors.ToArray()
                ),
                ResultStatus.Unavailable => Result<TDestination>.Unavailable(
                    result.Errors.ToArray()
                ),
                ResultStatus.NoContent => Result<TDestination>.NoContent(),
                _ => throw new NotSupportedException(
                    $"Result {result.Status} conversion is not supported."
                ),
            };
        }

        private static Result<TDestination> HandleNonSuccessStatus<TDestination>(Result result)
        {
            return result.Status switch
            {
                ResultStatus.NotFound => result.Errors.Any()
                    ? Result<TDestination>.NotFound(result.Errors.ToArray())
                    : Result<TDestination>.NotFound(),
                ResultStatus.Unauthorized => result.Errors.Any()
                    ? Result<TDestination>.Unauthorized(result.Errors.ToArray())
                    : Result<TDestination>.Unauthorized(),
                ResultStatus.Forbidden => result.Errors.Any()
                    ? Result<TDestination>.Forbidden(result.Errors.ToArray())
                    : Result<TDestination>.Forbidden(),
                ResultStatus.Invalid => Result<TDestination>.Invalid(result.ValidationErrors),
                ResultStatus.Error => Result<TDestination>.Error(
                    new ErrorList(result.Errors.ToArray(), result.CorrelationId)
                ),
                ResultStatus.Conflict => result.Errors.Any()
                    ? Result<TDestination>.Conflict(result.Errors.ToArray())
                    : Result<TDestination>.Conflict(),
                ResultStatus.CriticalError => Result<TDestination>.CriticalError(
                    result.Errors.ToArray()
                ),
                ResultStatus.Unavailable => Result<TDestination>.Unavailable(
                    result.Errors.ToArray()
                ),
                ResultStatus.NoContent => Result<TDestination>.NoContent(),
                _ => throw new NotSupportedException(
                    $"Result {result.Status} conversion is not supported."
                ),
            };
        }

        private static Result HandleNonSuccessStatus<TSource>(Result<TSource> result)
        {
            return result.Status switch
            {
                ResultStatus.NotFound => result.Errors.Any()
                    ? Result.NotFound(result.Errors.ToArray())
                    : Result.NotFound(),
                ResultStatus.Unauthorized => result.Errors.Any()
                    ? Result.Unauthorized(result.Errors.ToArray())
                    : Result.Unauthorized(),
                ResultStatus.Forbidden => result.Errors.Any()
                    ? Result.Forbidden(result.Errors.ToArray())
                    : Result.Forbidden(),
                ResultStatus.Invalid => Result.Invalid(result.ValidationErrors),
                ResultStatus.Error => Result.Error(
                    new ErrorList(result.Errors.ToArray(), result.CorrelationId)
                ),
                ResultStatus.Conflict => result.Errors.Any()
                    ? Result.Conflict(result.Errors.ToArray())
                    : Result.Conflict(),
                ResultStatus.CriticalError => Result.CriticalError(result.Errors.ToArray()),
                ResultStatus.Unavailable => Result.Unavailable(result.Errors.ToArray()),
                ResultStatus.NoContent => Result.NoContent(),
                _ => throw new NotSupportedException(
                    $"Result {result.Status} conversion is not supported."
                ),
            };
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
                case ResultStatus.Ok:
                    return Result.Success();
                case ResultStatus.NotFound:
                    return result.Errors.Any()
                        ? Result.NotFound(result.Errors.ToArray())
                        : Result.NotFound();
                case ResultStatus.Unauthorized:
                    return result.Errors.Any()
                        ? Result.Unauthorized(result.Errors.ToArray())
                        : Result.Unauthorized();
                case ResultStatus.Forbidden:
                    return result.Errors.Any()
                        ? Result.Forbidden(result.Errors.ToArray())
                        : Result.Forbidden();
                case ResultStatus.Invalid:
                    return Result.Invalid(result.ValidationErrors);
                case ResultStatus.Error:
                    return Result.Error(
                        new ErrorList(result.Errors.ToArray(), result.CorrelationId)
                    );
                case ResultStatus.Conflict:
                    return result.Errors.Any()
                        ? Result.Conflict(result.Errors.ToArray())
                        : Result.Conflict();
                case ResultStatus.CriticalError:
                    return Result.CriticalError(result.Errors.ToArray());
                case ResultStatus.Unavailable:
                    return Result.Unavailable(result.Errors.ToArray());
                case ResultStatus.NoContent:
                    return Result.NoContent();
                default:
                    throw new NotSupportedException(
                        $"Result {result.Status} conversion is not supported."
                    );
            }
        }
    }
}
