using Xunit;
using static Ardalis.Result.Result;

namespace Ardalis.Result.FluentAssertions.UnitTests.FailureResults;

public class UnavailableResult
{
    private const string ErrorMessage = "Error message"; 
    
    [Fact]
    public void UnavailableResult_ShouldBeFailure()
    {
        Unavailable().ShouldBeFailure();
    }

    [Fact]
    public void UnavailableResultWithErrorMessages_ShouldBeFailureWithErrorMessages()
    {
        Unavailable(ErrorMessage).ShouldBeFailure(ErrorMessage);
    }

    [Fact]
    public void UnavailableResult_ShouldBeUnavailable()
    {
        Unavailable().ShouldBeUnavailable();
    }

    [Fact]
    public void UnavailableResultWithErrorMessages_ShouldBeUnavailableWithErrorMessages()
    {
        Unavailable(ErrorMessage).ShouldBeUnavailable(ErrorMessage);
    }
}