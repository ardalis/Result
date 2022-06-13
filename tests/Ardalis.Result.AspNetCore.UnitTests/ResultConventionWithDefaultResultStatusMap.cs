using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Xunit;

namespace Ardalis.Result.AspNetCore.UnitTests;

public class ResultConventionWithDefaultResultStatusMap : BaseResultConventionTest
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

        Assert.Contains(actionModel.Filters, f => IsProducesResponseTypeAttribute(f, 200, typeof(void)));
        Assert.Contains(actionModel.Filters, f => IsProducesResponseTypeAttribute(f, 404, typeof(void)));
        Assert.Contains(actionModel.Filters, f => IsProducesResponseTypeAttribute(f, 400, typeof(IDictionary<string, string[]>)));
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

        Assert.Contains(actionModel.Filters, f => IsProducesResponseTypeAttribute(f, 200, typeof(void)));
        Assert.Contains(actionModel.Filters, f => IsProducesResponseTypeAttribute(f, 404, typeof(void)));
        Assert.Contains(actionModel.Filters, f => IsProducesResponseTypeAttribute(f, 400, typeof(IDictionary<string, string[]>)));
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
}
