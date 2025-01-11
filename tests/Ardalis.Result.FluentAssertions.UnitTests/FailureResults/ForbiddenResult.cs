using Xunit;
using static Ardalis.Result.Result;
using static Ardalis.Result.FluentAssertions.UnitTests.Utils.Constants;

namespace Ardalis.Result.FluentAssertions.UnitTests.FailureResults;

public class ForbiddenResult
{
    [Fact]
    public void ShouldBeFailure()
    {
        Forbidden().ShouldBeFailure();
    }

    [Fact]
    public void ShouldBeFailureWithMessage()
    {
        Forbidden(ErrorMessage).ShouldBeFailure(ErrorMessage);
    }

    [Fact]
    public void ShouldBeForbidden()
    {
        Forbidden().ShouldBeForbidden();
    }

    [Fact]
    public void ShouldBeForbiddenWithMessage()
    {
        Forbidden(ErrorMessage).ShouldBeForbidden(ErrorMessage);
    }
}