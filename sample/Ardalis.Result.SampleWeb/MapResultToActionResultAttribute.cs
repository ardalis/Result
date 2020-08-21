namespace Ardalis.Result.SampleWeb
{
    // This is left here in case you want to implement your own custom filter
    // Otherwise just use the Ardalis.Result.AspNetCore Nuget package
    //public class MapResultToActionResultAttribute : ActionFilterAttribute
    //{
    //    public override void OnActionExecuted(ActionExecutedContext context)
    //    {
    //        if (!((context.Result as ObjectResult).Value is IResult result)) return;

    //        if (!(context.Controller is ControllerBase controller)) return;

    //        if (result.Status == ResultStatus.NotFound)
    //            context.Result = controller.NotFound();

    //        if (result.Status == ResultStatus.Invalid)
    //        {
    //            foreach (var error in result.ValidationErrors)
    //            {
    //                (context.Controller as ControllerBase).ModelState.AddModelError(error.Key, error.Value);
    //            }

    //            context.Result = controller.BadRequest(controller.ModelState);
    //        }

    //        if(result.Status == ResultStatus.Ok)
    //        {
    //            context.Result = new OkObjectResult(result.GetValue());
    //        }
    //    }
    //}
}
