using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Ardalis.Result.AspNetCore
{
    public class TranslateResultToActionResultAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuted(ActionExecutedContext context)
        {
            if (!((context.Result as ObjectResult)?.Value is IResult result)) return;

            if (!(context.Controller is ControllerBase controller)) return;

            if (result.Status == ResultStatus.NotFound)
                context.Result = controller.NotFound();

            if (result.Status == ResultStatus.Invalid)
            {
                foreach (var error in result.ValidationErrors)
                {
                    foreach (var errorMessage in error.Value)
                    {
                        (context.Controller as ControllerBase)?.ModelState.AddModelError(error.Key, errorMessage);
                    }
                }

                context.Result = controller.BadRequest(controller.ModelState);
            }

            if(result.Status == ResultStatus.Ok)
            {
                context.Result = new OkObjectResult(result.GetValue());
            }
        }
    }
}
