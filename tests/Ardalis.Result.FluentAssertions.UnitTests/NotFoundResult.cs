using Xunit;

namespace Ardalis.Result.FluentAssertions.UnitTests;

public class NotFoundResult
{
    private const string ErrorMessage = "Error message"; 

    [Fact]
    public void NotFoundResult_ShouldBeFailure()
    {
        Result.NotFound().ShouldBeFailure();
    }

    [Fact]
    public void NotFoundResultWithMessage_ShouldBeFailureWithMessage()
    {
        Result.NotFound(ErrorMessage).ShouldBeFailure(ErrorMessage);
    }

    [Fact]
    public void NotFoundResult_ShouldBeNotFound()
    {
        Result.NotFound().ShouldBeNotFound();
    }

    [Fact]
    public void NotFoundResultWithErrorMessages_ShouldBeNotFoundWithErrorMessages()
    {
        Result.NotFound(ErrorMessage).ShouldBeNotFound(ErrorMessage);
    }
}