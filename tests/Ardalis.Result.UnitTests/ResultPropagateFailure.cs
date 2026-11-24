using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace Ardalis.Result.UnitTests;

public class ResultPropagateFailure
{
    private readonly string _testError = "Test error message";
    private readonly string _testValidationError = "Validation failed";

    #region Error Status

    [Fact]
    public void PropagateFailure_WithErrorStatus_PreservesErrorWithErrorList()
    {
        // Arrange
        var sourceResult = Result<int>.Error(new ErrorList(new[] { _testError }));

        // Act
        var result = sourceResult.PropagateFailure<int, string>();

        // Assert
        result.Status.Should().Be(ResultStatus.Error);
        result.IsError().Should().BeTrue();
        result.Errors.Should().Contain(_testError);
    }

    [Fact]
    public void PropagateFailure_WithErrorStatus_ConvertsFromIntToString()
    {
        // Arrange
        var sourceResult = Result<int>.Error(new ErrorList(new[] { _testError }));

        // Act
        var result = sourceResult.PropagateFailure<int, string>();

        // Assert
        result.Status.Should().Be(ResultStatus.Error);
        result.IsError().Should().BeTrue();
    }

    [Fact]
    public void PropagateFailure_WithMultipleErrors_PreservesAllErrors()
    {
        // Arrange
        var errors = new[] { "Error 1", "Error 2", "Error 3" };
        var sourceResult = Result<int>.Error(new ErrorList(errors));

        // Act
        var result = sourceResult.PropagateFailure<int, string>();

        // Assert
        result.IsError().Should().BeTrue();
        result.Errors.Should().HaveCount(3);
        result.Errors.Should().Equal(errors);
    }

    #endregion

    #region Forbidden Status

    [Fact]
    public void PropagateFailure_WithForbiddenStatus_PreservesForbidden()
    {
        // Arrange
        var sourceResult = Result<int>.Forbidden(new[] { _testError });

        // Act
        var result = sourceResult.PropagateFailure<int, string>();

        // Assert
        result.Status.Should().Be(ResultStatus.Forbidden);
        result.IsForbidden().Should().BeTrue();
        result.Errors.Should().Contain(_testError);
    }

    [Fact]
    public void PropagateFailure_WithForbiddenStatus_PreservesMultipleErrors()
    {
        // Arrange
        var errors = new[] { "Access denied", "Insufficient permissions" };
        var sourceResult = Result<int>.Forbidden(errors);

        // Act
        var result = sourceResult.PropagateFailure<int, string>();

        // Assert
        result.IsForbidden().Should().BeTrue();
        result.Errors.Should().Equal(errors);
    }

    #endregion

    #region Unauthorized Status

    [Fact]
    public void PropagateFailure_WithUnauthorizedStatus_PreservesUnauthorized()
    {
        // Arrange
        var sourceResult = Result<int>.Unauthorized(new[] { _testError });

        // Act
        var result = sourceResult.PropagateFailure<int, string>();

        // Assert
        result.Status.Should().Be(ResultStatus.Unauthorized);
        result.IsUnauthorized().Should().BeTrue();
        result.Errors.Should().Contain(_testError);
    }

    [Fact]
    public void PropagateFailure_WithUnauthorizedStatus_PreservesMultipleErrors()
    {
        // Arrange
        var errors = new[] { "Invalid credentials", "Token expired" };
        var sourceResult = Result<int>.Unauthorized(errors);

        // Act
        var result = sourceResult.PropagateFailure<int, string>();

        // Assert
        result.IsUnauthorized().Should().BeTrue();
        result.Errors.Should().Equal(errors);
    }

    #endregion

    #region Invalid Status

    [Fact]
    public void PropagateFailure_WithInvalidStatus_PreservesValidationErrors()
    {
        // Arrange
        var validationErrors = new List<ValidationError>
        {
            new() { ErrorMessage = _testValidationError }
        };
        var sourceResult = Result<int>.Invalid(validationErrors);

        // Act
        var result = sourceResult.PropagateFailure<int, string>();

        // Assert
        result.Status.Should().Be(ResultStatus.Invalid);
        result.IsInvalid().Should().BeTrue();
        result.ValidationErrors.Should().BeEquivalentTo(validationErrors);
    }

    [Fact]
    public void PropagateFailure_WithInvalidStatus_PreservesMultipleValidationErrors()
    {
        // Arrange
        var validationErrors = new List<ValidationError>
        {
            new() { ErrorMessage = "Invalid email format" },
            new() { ErrorMessage = "Password too short" },
            new() { ErrorMessage = "Must be at least 18" }
        };
        var sourceResult = Result<int>.Invalid(validationErrors);

        // Act
        var result = sourceResult.PropagateFailure<int, string>();

        // Assert
        result.IsInvalid().Should().BeTrue();
        result.ValidationErrors.Should().HaveCount(3);
        result.ValidationErrors.Should().BeEquivalentTo(validationErrors);
    }

    #endregion

    #region NotFound Status

    [Fact]
    public void PropagateFailure_WithNotFoundStatus_PreservesNotFound()
    {
        // Arrange
        var sourceResult = Result<int>.NotFound(new[] { _testError });

        // Act
        var result = sourceResult.PropagateFailure<int, string>();

        // Assert
        result.Status.Should().Be(ResultStatus.NotFound);
        result.IsNotFound().Should().BeTrue();
        result.Errors.Should().Contain(_testError);
    }

    [Fact]
    public void PropagateFailure_WithNotFoundStatus_PreservesMultipleErrors()
    {
        // Arrange
        var errors = new[] { "Resource not found", "Item does not exist" };
        var sourceResult = Result<int>.NotFound(errors);

        // Act
        var result = sourceResult.PropagateFailure<int, string>();

        // Assert
        result.IsNotFound().Should().BeTrue();
        result.Errors.Should().Equal(errors);
    }

    #endregion

    #region NoContent Status

    [Fact]
    public void PropagateFailure_WithNoContentStatus_PreservesNoContent()
    {
        // Arrange
        var sourceResult = Result<int>.NoContent();

        // Act
        var result = sourceResult.PropagateFailure<int, string>();

        // Assert
        result.Status.Should().Be(ResultStatus.NoContent);
        result.IsNoContent().Should().BeTrue();
    }

    #endregion

    #region Conflict Status

    [Fact]
    public void PropagateFailure_WithConflictStatus_PreservesConflict()
    {
        // Arrange
        var sourceResult = Result<int>.Conflict(new[] { _testError });

        // Act
        var result = sourceResult.PropagateFailure<int, string>();

        // Assert
        result.Status.Should().Be(ResultStatus.Conflict);
        result.IsConflict().Should().BeTrue();
        result.Errors.Should().Contain(_testError);
    }

    [Fact]
    public void PropagateFailure_WithConflictStatus_PreservesMultipleErrors()
    {
        // Arrange
        var errors = new[] { "Version conflict", "Resource already exists" };
        var sourceResult = Result<int>.Conflict(errors);

        // Act
        var result = sourceResult.PropagateFailure<int, string>();

        // Assert
        result.IsConflict().Should().BeTrue();
        result.Errors.Should().Equal(errors);
    }

    #endregion

    #region CriticalError Status

    [Fact]
    public void PropagateFailure_WithCriticalErrorStatus_PreservesCriticalError()
    {
        // Arrange
        var sourceResult = Result<int>.CriticalError(new[] { _testError });

        // Act
        var result = sourceResult.PropagateFailure<int, string>();

        // Assert
        result.Status.Should().Be(ResultStatus.CriticalError);
        result.IsCriticalError().Should().BeTrue();
        result.Errors.Should().Contain(_testError);
    }

    [Fact]
    public void PropagateFailure_WithCriticalErrorStatus_PreservesMultipleErrors()
    {
        // Arrange
        var errors = new[] { "Database connection failed", "System unavailable" };
        var sourceResult = Result<int>.CriticalError(errors);

        // Act
        var result = sourceResult.PropagateFailure<int, string>();

        // Assert
        result.IsCriticalError().Should().BeTrue();
        result.Errors.Should().Equal(errors);
    }

    #endregion

    #region Unavailable Status

    [Fact]
    public void PropagateFailure_WithUnavailableStatus_PreservesUnavailable()
    {
        // Arrange
        var sourceResult = Result<int>.Unavailable(new[] { _testError });

        // Act
        var result = sourceResult.PropagateFailure<int, string>();

        // Assert
        result.Status.Should().Be(ResultStatus.Unavailable);
        result.IsUnavailable().Should().BeTrue();
        result.Errors.Should().Contain(_testError);
    }

    [Fact]
    public void PropagateFailure_WithUnavailableStatus_PreservesMultipleErrors()
    {
        // Arrange
        var errors = new[] { "Service unavailable", "Server temporarily down" };
        var sourceResult = Result<int>.Unavailable(errors);

        // Act
        var result = sourceResult.PropagateFailure<int, string>();

        // Assert
        result.IsUnavailable().Should().BeTrue();
        result.Errors.Should().Equal(errors);
    }

    #endregion

    #region Type Conversion

    [Fact]
    public void PropagateFailure_ConvertsFromIntToString()
    {
        // Arrange
        var sourceResult = Result<int>.NotFound(new[] { _testError });

        // Act
        var result = sourceResult.PropagateFailure<int, string>();

        // Assert
        result.IsNotFound().Should().BeTrue();
    }

    [Fact]
    public void PropagateFailure_ConvertsFromStringToCustomType()
    {
        // Arrange
        var sourceResult = Result<string>.Forbidden(new[] { _testError });

        // Act
        var result = sourceResult.PropagateFailure<string, TestFoo>();

        // Assert
        result.IsForbidden().Should().BeTrue();
    }

    [Fact]
    public void PropagateFailure_ConvertsFromCustomTypeToAnotherCustomType()
    {
        // Arrange
        var sourceResult = Result<TestFoo>.Unauthorized(new[] { _testError });

        // Act
        var result = sourceResult.PropagateFailure<TestFoo, TestFooDto>();

        // Assert
        result.IsUnauthorized().Should().BeTrue();
    }

    #endregion

    #region Edge Cases

    [Fact]
    public void PropagateFailure_WithEmptyErrorArray_StillPreservesStatus()
    {
        // Arrange
        var sourceResult = Result<int>.NotFound(System.Array.Empty<string>());

        // Act
        var result = sourceResult.PropagateFailure<int, string>();

        // Assert
        result.IsNotFound().Should().BeTrue();
        result.Errors.Should().BeEmpty();
    }

    [Fact]
    public void PropagateFailure_WithSuccessStatus_ThrowsInvalidOperationException()
    {
        // Arrange
        var sourceResult = Result<int>.Success(42);

        // Act & Assert
        var exception = Record.Exception(() => sourceResult.PropagateFailure<int, string>());
        exception.Should().BeOfType<System.InvalidOperationException>();
        exception.Message.Should().Contain("Unexpected ResultStatus");
    }

    [Fact]
    public void PropagateFailure_WithCreatedStatus_ThrowsInvalidOperationException()
    {
        // Arrange
        var sourceResult = Result<int>.Created(42);

        // Act & Assert
        var exception = Record.Exception(() => sourceResult.PropagateFailure<int, string>());
        exception.Should().BeOfType<System.InvalidOperationException>();
        exception.Message.Should().Contain("Unexpected ResultStatus");
    }

    #endregion

    #region Fluent Chain

    [Fact]
    public void PropagateFailure_CanBeChainedWithOtherExtensions()
    {
        // Arrange
        var sourceResult = Result<int>.NotFound(new[] { "Item not found" });

        // Act
        var result = sourceResult
            .PropagateFailure<int, string>()
            .Map(_ => _ ?? "default");

        // Assert
        result.Status.Should().Be(ResultStatus.NotFound);
        result.IsNotFound().Should().BeTrue();
    }

    #endregion

    private record TestFoo(string Bar);

    private class TestFooDto
    {
        public string Bar { get; set; }

        public TestFooDto(string bar)
        {
            Bar = bar;
        }

        public static TestFooDto CreateFromFoo(TestFoo foo)
        {
            return new TestFooDto(foo.Bar);
        }
    }
}
