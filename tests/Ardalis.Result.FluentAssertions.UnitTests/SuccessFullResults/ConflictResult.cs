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
    public void ShouldBeSuccessWithMessage()
    {
        SuccessWithMessage("SUCCESS_MESSAGE").ShouldBeSuccessWithMessage("SUCCESS_MESSAGE");
    }
}