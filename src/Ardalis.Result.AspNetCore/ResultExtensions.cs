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
        public static ActionResult<T> ToActionResult<T>(this ControllerBase controller,
            Result<T> result)
        {
            if (result.Status == ResultStatus.NotFound) return controller.NotFound();
            if (result.Status == ResultStatus.Invalid)
            {
                foreach (var error in result.ValidationErrors)
                {
                    // TODO: Fix after updating to 3.0.0
                    controller.ModelState.AddModelError(error.Key, error.Value);
                }
                return controller.BadRequest(controller.ModelState);
            }

            return controller.Ok(result.Value);
        }
    }
}
