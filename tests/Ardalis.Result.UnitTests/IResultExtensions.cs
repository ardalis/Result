using Xunit;

namespace Ardalis.Result.UnitTests;

public class IResultExtensions
{
    [Fact]
    public void IsOk_ReturnsTrue_WhenStatusIsOk()
    {
        // Arrange & Act
        var result = Result.Success();

        // Assert
        Assert.True(result.IsOk());
    }

    [Fact]
    public void IsCreated_ReturnsTrue_WhenStatusIsCreated()
    {
        // Arrange & Act
        var foo = new Foo("Bar");
        var result = Result<Foo>.Created(foo);

        // Assert
        Assert.True(result.IsCreated());
    }

    [Fact]
    public void IsError_ReturnsTrue_WhenStatusIsError()
    {
        // Arrange & Act
        var result = Result.Error();

        // Assert
        Assert.True(result.IsError());
    }

    [Fact]
    public void IsForbidden_ReturnsTrue_WhenStatusIsForbidden()
    {
        // Arrange & Act
        var result = Result.Forbidden();

        // Assert
        Assert.True(result.IsForbidden());
    }

    [Fact]
    public void IsUnauthorized_ReturnsTrue_WhenStatusIsUnauthorized()
    {
        // Arrange & Act
        var result = Result.Unauthorized();

        // Assert
        Assert.True(result.IsUnauthorized());
    }

    [Fact]
    public void IsInvalid_ReturnsTrue_WhenStatusIsInvalid()
    {
        // Arrange & Act
        var result = Result.Invalid();

        // Assert
        Assert.True(result.IsInvalid());
    }

    [Fact]
    public void IsNotFound_ReturnsTrue_WhenStatusIsNotFound()
    {
        // Arrange & Act
        var result = Result.NotFound();

        // Assert
        Assert.True(result.IsNotFound());
    }

    [Fact]
    public void IsNoContent_ReturnsTrue_WhenStatusIsNoContent()
    {
        // Arrange & ActI
        var result = Result.NoContent();

        // Assert
        Assert.True(result.IsNoContent());
    }

    [Fact]
    public void IsConflict_ReturnsTrue_WhenStatusIsConflict()
    {
        // Arrange & Act
        var result = Result.Conflict();

        // Assert
        Assert.True(result.IsConflict());
    }

    [Fact]
    public void IsCriticalError_ReturnsTrue_WhenStatusIsCriticalError()
    {
        // Arrange & Act
        var result = Result.CriticalError();

        // Assert
        Assert.True(result.IsCriticalError());
    }

    [Fact]
    public void IsUnavailable_ReturnsTrue_WhenStatusIsUnavailable()
    {
        // Arrange & Act
        var result = Result.Unavailable();

        // Assert
        Assert.True(result.IsUnavailable());
    }

    private record Foo(string Value);

}
