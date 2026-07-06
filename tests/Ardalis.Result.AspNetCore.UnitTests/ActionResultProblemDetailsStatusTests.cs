using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;
using Xunit;

namespace Ardalis.Result.AspNetCore.UnitTests;

public class ActionResultProblemDetailsStatusTests
{
    [Fact]
    public void ErrorPopulatesProblemDetailsStatus() => AssertProblemDetailsStatus(Result<int>.Error("boom"), 422);

    [Fact]
    public void NotFoundPopulatesProblemDetailsStatus() => AssertProblemDetailsStatus(Result<int>.NotFound("missing"), 404);

    [Fact]
    public void ConflictPopulatesProblemDetailsStatus() => AssertProblemDetailsStatus(Result<int>.Conflict("conflict"), 409);

    [Fact]
    public void CriticalErrorPopulatesProblemDetailsStatus() => AssertProblemDetailsStatus(Result<int>.CriticalError("boom"), 500);

    [Fact]
    public void UnavailablePopulatesProblemDetailsStatus() => AssertProblemDetailsStatus(Result<int>.Unavailable("down"), 503);

    private static void AssertProblemDetailsStatus(Result<int> result, int expectedStatusCode)
    {
        var controller = new TestController
        {
            ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext(),
                ActionDescriptor = new ControllerActionDescriptor()
            }
        };

        var actionResult = controller.ToActionResult(result);

        var objectResult = Assert.IsType<ObjectResult>(actionResult.Result);
        var problemDetails = Assert.IsType<ProblemDetails>(objectResult.Value);
        Assert.Equal(expectedStatusCode, problemDetails.Status);
        Assert.Equal(expectedStatusCode, objectResult.StatusCode);
    }

    private class TestController : ControllerBase;
}