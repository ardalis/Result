using System.Collections.Generic;
using Ardalis.Result.AspNetCore.UnitTests.Mocks;
using Microsoft.AspNetCore.Mvc;
using Xunit;

namespace Ardalis.Result.AspNetCore.UnitTests
{
    public class ToActionResultResultExtension
    {
        [Fact]
        public void ReturnsOkObjectResult()
        {
            var controller = new MockController();
            var result = new Result<object>(new object());

            var actionResult = result.ToActionResult(controller);
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

            var actionResult = result.ToActionResult(controller);
            var errorCount = controller.ModelState.ErrorCount;

            Assert.Equal(errorCount, 1);
            Assert.IsType<BadRequestObjectResult>(actionResult.Result);
        }

        [Fact]
        public void ReturnsNotFoundObjectResult()
        {
            var controller = new MockController();
            var result = Result<object>.NotFound();

            var actionResult = result.ToActionResult(controller);
            Assert.IsType<NotFoundResult>(actionResult.Result);
        }
    }
}
