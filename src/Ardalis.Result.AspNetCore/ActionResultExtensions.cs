using System;
using System.Linq;
using System.Text;
using Microsoft.AspNetCore.Mvc;

namespace Ardalis.Result.AspNetCore
{
    /// <summary>
    /// Extensions to support converting Result to an ActionResult
    /// </summary>
    public static partial class ResultExtensions
    {
        /// <summary>
        /// Convert a <see cref="Result{T}"/> to a <see cref="ActionResult"/>
        /// </summary>
        /// <typeparam name="T">The value being returned</typeparam>
        /// <param name="controller">The controller this is called from</param>
        /// <param name="result">The Result to convert to an ActionResult</param>
        /// <returns></returns>
        public static ActionResult<T> ToActionResult<T>(this Result<T> result, ControllerBase controller)
        {
            return controller.ToActionResult((IResult)result);
        }

        /// <summary>
        /// Convert a <see cref="Result"/> to a <see cref="ActionResult"/>
        /// </summary>
        /// <param name="controller">The controller this is called from</param>
        /// <param name="result">The Result to convert to an ActionResult</param>
        /// <returns></returns>
        public static ActionResult ToActionResult(this Result result, ControllerBase controller)
        {
            return controller.ToActionResult((IResult)result);
        }

        /// <summary>
        /// Convert a <see cref="Result{T}"/> to a <see cref="ActionResult"/>
        /// </summary>
        /// <typeparam name="T">The value being returned</typeparam>
        /// <param name="controller">The controller this is called from</param>
        /// <param name="result">The Result to convert to an ActionResult</param>
        /// <returns></returns>
        public static ActionResult<T> ToActionResult<T>(this ControllerBase controller,
            Result<T> result)
        {
            return controller.ToActionResult((IResult)result);
        }

        /// <summary>
        /// Convert a <see cref="Result"/> to a <see cref="ActionResult"/>
        /// </summary>
        /// <param name="controller">The controller this is called from</param>
        /// <param name="result">The Result to convert to an ActionResult</param>
        /// <returns></returns>
        public static ActionResult ToActionResult(this ControllerBase controller,
            Result result)
        {
            return controller.ToActionResult((IResult)result);
        }

        internal static ActionResult ToActionResult(this ControllerBase controller, IResult result)
        {
            switch (result.Status)
            {
                case ResultStatus.Ok: return typeof(Result).IsInstanceOfType(result)
                        ? (ActionResult)controller.Ok() 
                        : controller.Ok(result.GetValue());
                case ResultStatus.NotFound: return NotFoundEntity(controller, result);
                case ResultStatus.Unauthorized: return controller.Unauthorized();
                case ResultStatus.Forbidden: return controller.Forbid();
                case ResultStatus.Invalid: return BadRequest(controller, result);
                case ResultStatus.Error: return UnprocessableEntity(controller, result);
                default:
                    throw new NotSupportedException($"Result {result.Status} conversion is not supported.");
            }
        }

        private static ActionResult BadRequest(ControllerBase controller, IResult result)
        {
            foreach (var error in result.ValidationErrors)
            {
                controller.ModelState.AddModelError(error.Identifier, error.ErrorMessage);
            }

            return controller.BadRequest(controller.ModelState);
        }

        private static ActionResult UnprocessableEntity(ControllerBase controller, IResult result)
        {
            var details = new StringBuilder("Next error(s) occured:");

            foreach (var error in result.Errors) details.Append("* ").Append(error).AppendLine();

            return controller.UnprocessableEntity(new ProblemDetails
            {
                Title = "Something went wrong.",
                Detail = details.ToString()
            });
        }

        private static ActionResult NotFoundEntity(ControllerBase controller, IResult result)
        {
            var details = new StringBuilder("Next error(s) occured:");

            if (result.Errors.Any())
            {
                foreach (var error in result.Errors) details.Append("* ").Append(error).AppendLine();

                return controller.NotFound(new ProblemDetails
                {
                    Title = "Resource not found.",
                    Detail = details.ToString()
                });
            }
            else
            {
                return controller.NotFound();
            }
        }
    }
}
