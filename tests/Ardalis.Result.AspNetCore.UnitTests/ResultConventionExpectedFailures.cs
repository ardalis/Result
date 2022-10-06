using Ardalis.Result.AspNetCore.Exceptions;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Xunit;

namespace Ardalis.Result.AspNetCore.UnitTests
{
    public class ResultConventionExpectedFailures : BaseResultConventionTest
    {
        [Fact]
        public void DefaultResultStatusMap()
        {
            var convention = new ResultConvention(new ResultStatusMap().AddDefaultMap());

            var actionModelBuilder = new ActionModelBuilder()
                .AddActionFilter(new TranslateResultToActionResultAttribute())
                .AddActionAttribute(new ExpectedFailuresAttribute(ResultStatus.NotFound, ResultStatus.Invalid));

            var actionModel = actionModelBuilder.GetActionModel();

            convention.Apply(actionModel);

            Assert.Equal(3, actionModel.Filters.Where(f => f is ProducesResponseTypeAttribute).Count());

            Assert.Contains(actionModel.Filters, f => IsProducesResponseTypeAttribute(f, 204, typeof(void)));
            Assert.Contains(actionModel.Filters, f => IsProducesResponseTypeAttribute(f, 404, typeof(ProblemDetails)));
            Assert.Contains(actionModel.Filters, f => IsProducesResponseTypeAttribute(f, 400, typeof(ValidationProblemDetails)));
        }

        [Fact]
        public void DefaultResultStatusMap_UnexpectedResults()
        {
            var convention = new ResultConvention(new ResultStatusMap()
                .For(ResultStatus.Ok, System.Net.HttpStatusCode.OK)
                .For(ResultStatus.NotFound, System.Net.HttpStatusCode.NotFound));

            var actionModelBuilder = new ActionModelBuilder()
                .AddActionFilter(new TranslateResultToActionResultAttribute())
                .AddActionAttribute(new ExpectedFailuresAttribute(ResultStatus.NotFound, ResultStatus.Invalid, ResultStatus.Error));

            var actionModel = actionModelBuilder.GetActionModel();

            var exception = Assert.Throws<UnexpectedFailureResultsException>(() => convention.Apply(actionModel));

            Assert.Contains(exception.UnexpectedStatuses, s => s == ResultStatus.Invalid);
            Assert.Contains(exception.UnexpectedStatuses, s => s == ResultStatus.Error);
        }
    }
}
