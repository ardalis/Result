using Microsoft.AspNetCore.Mvc;

namespace Ardalis.Result.SampleWeb.Extensions
{
    public static class ResultExtensions
    {

        public static ActionResult<T> ToActionResult<T>(this ControllerBase controller, Result<T> result)
        {
            if (result.Status == ResultStatus.NotFound) return controller.NotFound();
            if (result.Status == ResultStatus.Invalid)
            {
                foreach (var error in result.ValidationErrors)
                {
                    controller.ModelState.AddModelError(error.Key, error.Value);
                }
                return  controller.BadRequest(controller.ModelState);
            }

            return controller.Ok(result.Value);
        }
        
    }
}
