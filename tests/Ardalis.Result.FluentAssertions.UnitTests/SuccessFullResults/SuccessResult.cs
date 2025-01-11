using Moq;
using Xunit;
using static Ardalis.Result.Result;
using static Ardalis.Result.FluentAssertions.UnitTests.Utils.Constants;

namespace Ardalis.Result.FluentAssertions.UnitTests.SuccessFullResults;

public class SuccessResult
{
    [Fact]
    public void ShouldBeSuccess()
    {
        Success().ShouldBeSuccess();
    }
    
    [Fact]
    public void SuccessWithMessage_ShouldBeSuccessWithMessage()
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
        Created(It.IsAny<object>()).ShouldBeSuccess();
    }
    
    [Fact]
    public void CreatedWithLocation_ShouldBeCreatedWithLocation()
    {
        Created(It.IsAny<object>(), "LOCATION").ShouldBeCreatedWithLocation("LOCATION");
    }
}