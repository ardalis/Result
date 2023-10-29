using FluentAssertions;
using System.Collections.Generic;
using Xunit;

namespace Ardalis.Result.UnitTests;

public class ResultVoidToResultOfT
{
    [Theory]
    [InlineData("test1")]
    [InlineData("test1", "test2")]
    public void ConvertFromErrorResultOfUnit(params string[] errors)
    {
        var result = DoBusinessOperationExample<object>(Result.Error(errors));

        Assert.Null(result.Value);
        Assert.Equal(ResultStatus.Error, result.Status);

        foreach (var error in errors)
        {
            result.Errors.Should().ContainEquivalentOf(error);
        }
    }

    [Fact]
    public void ConvertFromInvalidResultOfUnitWithValidationErrorList()
    {
        var validationErrors = new List<ValidationError>
            {
                new ValidationError
                {
                    Identifier = "name",
                    ErrorMessage = "Name is required"
                },
                new ValidationError
                {
                    Identifier = "postalCode",
                    ErrorMessage = "PostalCode cannot exceed 10 characters"
                }
            };

        var result = DoBusinessOperationExample<object>(Result.Invalid(validationErrors));

        Assert.Null(result.Value);
        Assert.Equal(ResultStatus.Invalid, result.Status);

        result.Status.Should().Be(ResultStatus.Invalid);
        result.ValidationErrors.Should().ContainEquivalentOf(new ValidationError { ErrorMessage = "Name is required", Identifier = "name" });
        result.ValidationErrors.Should().ContainEquivalentOf(new ValidationError { ErrorMessage = "PostalCode cannot exceed 10 characters", Identifier = "postalCode" });
    }

    [Fact]
    public void ConvertFromInvalidResultOfUnitWithValidationError()
    {
        var validationError = new ValidationError
        {
            Identifier = "name",
            ErrorMessage = "Name is required"
        };

        var result = DoBusinessOperationExample<object>(Result.Invalid(validationError));

        Assert.Null(result.Value);
        Assert.Equal(ResultStatus.Invalid, result.Status);

        result.Status.Should().Be(ResultStatus.Invalid);
        result.ValidationErrors.Should().ContainEquivalentOf(new ValidationError { ErrorMessage = "Name is required", Identifier = "name" });
    }

    [Fact]
    public void ConvertFromNotFoundResultOfUnit()
    {
        var result = DoBusinessOperationExample<object>(Result.NotFound());

        Assert.Null(result.Value);
        Assert.Equal(ResultStatus.NotFound, result.Status);
    }

    [Fact]
    public void ConvertFromForbiddenResultOfUnit()
    {
        var result = DoBusinessOperationExample<object>(Result.Forbidden());

        Assert.Null(result.Value);
        Assert.Equal(ResultStatus.Forbidden, result.Status);
    }

    [Fact]
    public void ConvertFromUnauthorizedResultOfUnit()
    {
        var result = DoBusinessOperationExample<object>(Result.Unauthorized());

        Assert.Null(result.Value);
        Assert.Equal(ResultStatus.Unauthorized, result.Status);
    }
    
    [Fact]
    public void ConvertFromConflictResultOfUnit()
    {
        var result = DoBusinessOperationExample<object>(Result.Conflict());

        result.Status.Should().Be(ResultStatus.Conflict);
        result.Value.Should().BeNull();
    }

    [Fact]
    public void ConvertFromUnavailableResultOfUnit()
    {
        var result = DoBusinessOperationExample<object>(Result.Unavailable());

        result.Status.Should().Be(ResultStatus.Unavailable);
        result.Value.Should().BeNull();
    }

    [Fact]
    public void ConvertFromCriticalErrorResultOfUnit()
    {
        var result = DoBusinessOperationExample<object>(Result.CriticalError());

        result.Status.Should().Be(ResultStatus.CriticalError);
        result.Value.Should().BeNull();
    }

    public Result<T> DoBusinessOperationExample<T>(Result testValue) => testValue;
}
