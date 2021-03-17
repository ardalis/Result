using System.Collections.Generic;
using Ardalis.Result.AspNetCore.UnitTests.Mocks;
using Microsoft.AspNetCore.Mvc;
using Xunit;

namespace Ardalis.Result.AspNetCore.UnitTests
{
    public class ToActionResultControllerExtention
    {
        [Fact]
        public void ReturnsOkObjectResult()
        {
            var controller = new MockController();
            var result = new Result<object>(new object());

            var actionResult = controller.ToActionResult(result);
            Assert.IsType<OkObjectResult>(actionResult.Result);
        }

        [Fact]
        public void ReturnsInvalidObjectResult()
        {
            var controller = new MockController();
            var result = Result<object>.Invalid(new List<ValidationError>
            {
                new ValidationError
                {
                    ErrorMessage = "some error",
                    Identifier = "test identitifier",
                    Severity = ValidationSeverity.Info
                }
            });

            var actionResult = controller.ToActionResult(result);
            var errorCount = controller.ModelState.ErrorCount;

            Assert.Equal(errorCount, 1);
            Assert.IsType<BadRequestObjectResult>(actionResult.Result);
        }

        [Fact]
        public void ReturnsNotFoundObjectResult()
        {
            var controller = new MockController();
            var result = Result<object>.NotFound();

            var actionResult = controller.ToActionResult(result);
            Assert.IsType<NotFoundResult>(actionResult.Result);
        }
    }
}
