using Microsoft.AspNetCore.Mvc;
using System.Net;
using Xunit;

namespace Ardalis.Result.AspNetCore.UnitTests;

public class ResultConventionWithDefaultResultStatusMapModified : BaseResultConventionTest
{
    [Fact]
    public void RemoveResultStatus()
    {
        var convention = new ResultConvention(new ResultStatusMap()
            .AddDefaultMap()
            .Remove(ResultStatus.Unauthorized)
            .Remove(ResultStatus.Forbidden));

        var actionModelBuilder = new ActionModelBuilder()
            .AddActionFilter(new TranslateResultToActionResultAttribute());

        var actionModel = actionModelBuilder.GetActionModel();

        convention.Apply(actionModel);

        Assert.Equal(4, actionModel.Filters.Where(f => f is ProducesResponseTypeAttribute).Count());

        Assert.Contains(actionModel.Filters, f => IsProducesResponseTypeAttribute(f, 200, typeof(void)));
        Assert.Contains(actionModel.Filters, f => IsProducesResponseTypeAttribute(f, 404, typeof(void)));
        Assert.Contains(actionModel.Filters, f => IsProducesResponseTypeAttribute(f, 400, typeof(IDictionary<string, string[]>)));
        Assert.Contains(actionModel.Filters, f => IsProducesResponseTypeAttribute(f, 422, typeof(ProblemDetails)));
    }

    [Fact]
    public void ChangeResultStatus()
    {
        var convention = new ResultConvention(new ResultStatusMap()
            .AddDefaultMap()
            .For(ResultStatus.Error, System.Net.HttpStatusCode.InternalServerError));

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
        Assert.Contains(actionModel.Filters, f => IsProducesResponseTypeAttribute(f, 500, typeof(void)));
    }

    [Theory]
    [InlineData(typeof(HttpPostAttribute), 201)]
    [InlineData(typeof(HttpDeleteAttribute), 204)]
    [InlineData(typeof(HttpGetAttribute), 200)]
    public void ChangeResultStatus_ForSpecificMethods(Type type, int expectedStatusCode)
    {
        var convention = new ResultConvention(new ResultStatusMap()
            .AddDefaultMap()
            .For(ResultStatus.Ok, HttpStatusCode.OK, opts => opts
                .Override("POST", HttpStatusCode.Created)
                .Override("DELETE", HttpStatusCode.NoContent)));

        var actionModelBuilder = new ActionModelBuilder()
            .AddActionFilter(new TranslateResultToActionResultAttribute())
            .AddActionAttribute((Attribute)Activator.CreateInstance(type)!);

        var actionModel = actionModelBuilder.GetActionModel();

        convention.Apply(actionModel);

        Assert.Equal(6, actionModel.Filters.Where(f => f is ProducesResponseTypeAttribute).Count());

        Assert.Contains(actionModel.Filters, f => IsProducesResponseTypeAttribute(f, expectedStatusCode, typeof(void)));
        Assert.Contains(actionModel.Filters, f => IsProducesResponseTypeAttribute(f, 404, typeof(void)));
        Assert.Contains(actionModel.Filters, f => IsProducesResponseTypeAttribute(f, 400, typeof(IDictionary<string, string[]>)));
        Assert.Contains(actionModel.Filters, f => IsProducesResponseTypeAttribute(f, 401, typeof(void)));
        Assert.Contains(actionModel.Filters, f => IsProducesResponseTypeAttribute(f, 403, typeof(void)));
        Assert.Contains(actionModel.Filters, f => IsProducesResponseTypeAttribute(f, 422, typeof(ProblemDetails)));
    }
}
