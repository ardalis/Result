using Xunit;
using static Ardalis.Result.Result;

namespace Ardalis.Result.Shouldly.UnitTests.SuccessFullResults;

public class SuccessResult
{
    [Fact]
    public void ShouldBeSuccess()
    {
        Success().ShouldBeSuccess();
    }

    [Fact]
    public void ShouldBeSuccessWithMessage()
    {
        SuccessWithMessage("SUCCESS_MESSAGE").ShouldBeSuccessWithMessage("SUCCESS_MESSAGE");
    }

    [Fact]
    public void SuccessWithMessage_ShouldBeSuccess()
    {
        SuccessWithMessage("SUCCESS_MESSAGE").ShouldBeSuccess();
    }

    [Fact]
    public void NoContent_ShouldBeSuccess()
    {
        NoContent().ShouldBeSuccess();
    }

    [Fact]
    public void Created_ShouldBeSuccess()
    {
        Created(new object()).ShouldBeSuccess();
    }

    [Fact]
    public void CreatedWithLocation_ShouldBeCreatedWithLocation()
    {
        Created(new object(), "LOCATION").ShouldBeCreatedWithLocation("LOCATION");
    }
}