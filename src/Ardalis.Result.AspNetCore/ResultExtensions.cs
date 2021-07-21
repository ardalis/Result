﻿using System;
using System.Text;
using Microsoft.AspNetCore.Mvc;

namespace Ardalis.Result.AspNetCore
{
    /// <summary>
    /// Extensions to support converting Result to an ActionResult
    /// </summary>
    public static class ResultExtensions
    {
        /// <summary>
        /// Convert an Ardalis.Result to a Microsoft.AspNetCore.Mvc.ActionResult
        /// </summary>
        /// <typeparam name="T">The value being returned</typeparam>
        /// <param name="controller">The controller this is called from</param>
        /// <param name="result">The Result to convert to an ActionResult</param>
        /// <returns></returns>
        public static ActionResult<T> ToActionResult<T>(this Result<T> result, ControllerBase controller)
        {
            return controller.ToActionResult(result);
        }

        /// <summary>
        /// Convert an Ardalis.Result to a Microsoft.AspNetCore.Mvc.ActionResult
        /// </summary>
        /// <typeparam name="T">The value being returned</typeparam>
        /// <param name="controller">The controller this is called from</param>
        /// <param name="result">The Result to convert to an ActionResult</param>
        /// <returns></returns>
        public static ActionResult<T> ToActionResult<T>(this ControllerBase controller,
            Result<T> result)
        {
            switch (result.Status)
            {
                case ResultStatus.Ok: return controller.Ok(result.Value);
                case ResultStatus.NotFound: return controller.NotFound();
                case ResultStatus.Forbidden: return controller.Forbid();
                case ResultStatus.Invalid: return BadRequest(controller, result);
                case ResultStatus.Error: return UnprocessableEntity(controller, result);
                default:
                    throw new NotSupportedException($"Result {result.Status} conversion is not supported.");
            }
        }

        private static ActionResult<T> BadRequest<T>(ControllerBase controller, Result<T> result)
        {
            foreach (var error in result.ValidationErrors)
            {
                // TODO: Fix after updating to 3.0.0
                controller.ModelState.AddModelError(error.Identifier, error.ErrorMessage);
            }

            return controller.BadRequest(controller.ModelState);
        }

        private static ActionResult<T> UnprocessableEntity<T>(ControllerBase controller, Result<T> result)
        {
            var details = new StringBuilder("Next error(s) occured:");

            foreach (var error in result.Errors) details.Append("* ").Append(error).AppendLine();

            return controller.UnprocessableEntity(new ProblemDetails
            {
                Title = "Something went wrong.",
                Detail = details.ToString()
            });
        }
    }
}
