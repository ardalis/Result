using System;
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
            var actionProps = controller.ControllerContext.ActionDescriptor.Properties;

            var resultStatusMap = actionProps.ContainsKey(ResultConvention.RESULT_STATUS_MAP_PROP) 
                ?(actionProps[ResultConvention.RESULT_STATUS_MAP_PROP] as ResultStatusMap)
                : new ResultStatusMap().AddDefaultMap();

            var resultStatusOptions = resultStatusMap[result.Status];
            var statusCode = (int)resultStatusOptions.GetStatusCode(controller.HttpContext.Request.Method);

            switch (result.Status)
            {
                case ResultStatus.Ok:
                    return typeof(Result).IsInstanceOfType(result)
                        ? (ActionResult)controller.StatusCode(statusCode)
                        : controller.StatusCode(statusCode, result.GetValue());
                default:
                    return resultStatusOptions.ResponseType == null
                        ? (ActionResult)controller.StatusCode(statusCode)
                        : controller.StatusCode(statusCode, resultStatusOptions.GetResponseObject(controller, result));
            }
        }
    }
}
