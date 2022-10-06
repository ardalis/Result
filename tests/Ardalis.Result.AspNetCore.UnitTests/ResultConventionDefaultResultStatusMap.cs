using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Xunit;

namespace Ardalis.Result.AspNetCore.UnitTests;

public class ResultConventionDefaultResultStatusMap : BaseResultConventionTest
{
    [Fact]
    public void TranslateAttributeOnAction()
    {
        var convention = new ResultConvention(new ResultStatusMap().AddDefaultMap());

        var actionModelBuilder = new ActionModelBuilder()
            .AddActionFilter(new TranslateResultToActionResultAttribute());

        var actionModel = actionModelBuilder.GetActionModel();

        convention.Apply(actionModel);

        Assert.Equal(6, actionModel.Filters.Where(f => f is ProducesResponseTypeAttribute).Count());

        Assert.Contains(actionModel.Filters, f => IsProducesResponseTypeAttribute(f, 204, typeof(void)));
        Assert.Contains(actionModel.Filters, f => IsProducesResponseTypeAttribute(f, 404, typeof(ProblemDetails)));
        Assert.Contains(actionModel.Filters, f => IsProducesResponseTypeAttribute(f, 400, typeof(ValidationProblemDetails)));
        Assert.Contains(actionModel.Filters, f => IsProducesResponseTypeAttribute(f, 401, typeof(void)));
        Assert.Contains(actionModel.Filters, f => IsProducesResponseTypeAttribute(f, 403, typeof(void)));
        Assert.Contains(actionModel.Filters, f => IsProducesResponseTypeAttribute(f, 422, typeof(ProblemDetails)));
    }

    [Fact]
    public void TranslateAttributeOnController()
    {
        var convention = new ResultConvention(new ResultStatusMap().AddDefaultMap());

        var actionModelBuilder = new ActionModelBuilder()
            .AddControllerFilter(new TranslateResultToActionResultAttribute());

        var actionModel = actionModelBuilder.GetActionModel();

        convention.Apply(actionModel);

        Assert.Equal(6, actionModel.Filters.Where(f => f is ProducesResponseTypeAttribute).Count());

        Assert.Contains(actionModel.Filters, f => IsProducesResponseTypeAttribute(f, 204, typeof(void)));
        Assert.Contains(actionModel.Filters, f => IsProducesResponseTypeAttribute(f, 404, typeof(ProblemDetails)));
        Assert.Contains(actionModel.Filters, f => IsProducesResponseTypeAttribute(f, 400, typeof(ValidationProblemDetails)));
        Assert.Contains(actionModel.Filters, f => IsProducesResponseTypeAttribute(f, 401, typeof(void)));
        Assert.Contains(actionModel.Filters, f => IsProducesResponseTypeAttribute(f, 403, typeof(void)));
        Assert.Contains(actionModel.Filters, f => IsProducesResponseTypeAttribute(f, 422, typeof(ProblemDetails)));
    }

    [Fact]
    public void WithoutTranslateAttribute()
    {
        var convention = new ResultConvention(new ResultStatusMap().AddDefaultMap());

        var actionModelBuilder = new ActionModelBuilder();

        var actionModel = actionModelBuilder.GetActionModel();

        convention.Apply(actionModel);

        Assert.Empty(actionModel.Filters.Where(f => f is ProducesResponseTypeAttribute));
    }

    [Fact]
    public void ExistingProducesResponseTypeAttributePreserved()
    {
        var convention = new ResultConvention(new ResultStatusMap().AddDefaultMap());

        var actionModelBuilder = new ActionModelBuilder()
            .AddActionFilter(new TranslateResultToActionResultAttribute())
            .AddActionFilter(new ProducesResponseTypeAttribute(typeof(IEnumerable<string>), 400));

        var actionModel = actionModelBuilder.GetActionModel();

        convention.Apply(actionModel);

        Assert.Equal(6, actionModel.Filters.Where(f => f is ProducesResponseTypeAttribute).Count());

        Assert.Contains(actionModel.Filters, f => IsProducesResponseTypeAttribute(f, 204, typeof(void)));
        Assert.Contains(actionModel.Filters, f => IsProducesResponseTypeAttribute(f, 404, typeof(ProblemDetails)));
        Assert.Contains(actionModel.Filters, f => IsProducesResponseTypeAttribute(f, 400, typeof(IEnumerable<string>)));
        Assert.Contains(actionModel.Filters, f => IsProducesResponseTypeAttribute(f, 401, typeof(void)));
        Assert.Contains(actionModel.Filters, f => IsProducesResponseTypeAttribute(f, 403, typeof(void)));
        Assert.Contains(actionModel.Filters, f => IsProducesResponseTypeAttribute(f, 422, typeof(ProblemDetails)));
    }

    [Theory]
    [InlineData(nameof(TestController.ResultString), typeof(string))]
    [InlineData(nameof(TestController.ResultEnumerableString), typeof(IEnumerable<string>))]
    public void ResultWithValue(string actionName, Type expectedType)
    {
        var convention = new ResultConvention(new ResultStatusMap().AddDefaultMap());

        var actionModelBuilder = new ActionModelBuilder()
            .AddActionFilter(new TranslateResultToActionResultAttribute());

        var actionModel = actionModelBuilder.GetActionModel(actionName);

        convention.Apply(actionModel);

        Assert.Equal(6, actionModel.Filters.Where(f => f is ProducesResponseTypeAttribute).Count());

        Assert.Contains(actionModel.Filters, f => IsProducesResponseTypeAttribute(f, 200, expectedType));
        Assert.Contains(actionModel.Filters, f => IsProducesResponseTypeAttribute(f, 404, typeof(ProblemDetails)));
        Assert.Contains(actionModel.Filters, f => IsProducesResponseTypeAttribute(f, 400, typeof(ValidationProblemDetails)));
        Assert.Contains(actionModel.Filters, f => IsProducesResponseTypeAttribute(f, 401, typeof(void)));
        Assert.Contains(actionModel.Filters, f => IsProducesResponseTypeAttribute(f, 403, typeof(void)));
        Assert.Contains(actionModel.Filters, f => IsProducesResponseTypeAttribute(f, 422, typeof(ProblemDetails)));
    }
}
