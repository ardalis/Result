using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;
using System.Net;
using Xunit;

namespace Ardalis.Result.AspNetCore.UnitTests;

public class ResultExtensionsToActionResult
{
    [Fact]
    public void Ok_UsesConfiguredResponseObject()
    {
        var controller = CreateController(new ResultStatusMap()
            .AddDefaultMap()
            .For(ResultStatus.Ok, HttpStatusCode.OK, opts => opts
                .With((ctrl, result) => new Envelope(result.GetValue()))));

        var actionResult = controller.ToActionResult(Result<string>.Success("hello"));

        var objectResult = Assert.IsAssignableFrom<ObjectResult>(actionResult.Result);
        Assert.Equal(200, objectResult.StatusCode);
        var envelope = Assert.IsType<Envelope>(objectResult.Value);
        Assert.Equal("hello", envelope.Data);
    }

    [Fact]
    public void Ok_ForNonGenericResult_UsesConfiguredResponseObject()
    {
        var controller = CreateController(new ResultStatusMap()
            .AddDefaultMap()
            .For(ResultStatus.Ok, HttpStatusCode.OK, opts => opts
                .With((ctrl, result) => new Envelope("custom"))));

        var actionResult = controller.ToActionResult(Result.Success());

        var objectResult = Assert.IsAssignableFrom<ObjectResult>(actionResult);
        Assert.Equal(200, objectResult.StatusCode);
        var envelope = Assert.IsType<Envelope>(objectResult.Value);
        Assert.Equal("custom", envelope.Data);
    }

    [Fact]
    public void Ok_WithoutConfiguredResponseObject_ReturnsValue()
    {
        var controller = CreateController(new ResultStatusMap().AddDefaultMap());

        var actionResult = controller.ToActionResult(Result<string>.Success("hello"));

        var objectResult = Assert.IsAssignableFrom<ObjectResult>(actionResult.Result);
        Assert.Equal(200, objectResult.StatusCode);
        Assert.Equal("hello", objectResult.Value);
    }

    [Fact]
    public void Created_UsesConfiguredResponseObject()
    {
        var controller = CreateController(new ResultStatusMap()
            .AddDefaultMap()
            .For(ResultStatus.Created, HttpStatusCode.Created, opts => opts
                .With((ctrl, result) => new Envelope(result.GetValue()))));

        var actionResult = controller.ToActionResult(Result<string>.Created("hello", "/items/1"));

        var createdResult = Assert.IsType<CreatedResult>(actionResult.Result);
        Assert.EndsWith("/items/1", createdResult.Location);
        var envelope = Assert.IsType<Envelope>(createdResult.Value);
        Assert.Equal("hello", envelope.Data);
    }

    [Fact]
    public void Created_WithoutConfiguredResponseObject_ReturnsValue()
    {
        var controller = CreateController(new ResultStatusMap().AddDefaultMap());

        var actionResult = controller.ToActionResult(Result<string>.Created("hello", "/items/1"));

        var createdResult = Assert.IsType<CreatedResult>(actionResult.Result);
        Assert.EndsWith("/items/1", createdResult.Location);
        Assert.Equal("hello", createdResult.Value);
    }

    private static TestController CreateController(ResultStatusMap map)
    {
        var httpContext = new DefaultHttpContext();
        httpContext.Request.Scheme = "https";
        httpContext.Request.Host = new HostString("localhost");

        var controller = new TestController
        {
            ControllerContext = new ControllerContext
            {
                HttpContext = httpContext,
                ActionDescriptor = new ControllerActionDescriptor()
            }
        };
        controller.ControllerContext.ActionDescriptor.Properties[ResultConvention.RESULT_STATUS_MAP_PROP] = map;

        return controller;
    }

    public class Envelope
    {
        public Envelope(object data) => Data = data;

        public object Data { get; }
    }

    public class TestController : ControllerBase
    {
    }
}
