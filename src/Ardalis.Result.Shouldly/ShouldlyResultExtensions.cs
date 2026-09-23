#nullable enable

using System.Collections.Generic;
using Shouldly;

namespace Ardalis.Result.Shouldly;

public static class ShouldlyResultExtensions
{
    public static void ShouldBeFailure(this Result result)
    {
        result.IsSuccess.ShouldBeFalse();
    }

    public static void ShouldBeFailure(this Result result, params string[] errorMessages)
    {
        result.ShouldBeFailure();
        result.Errors.ShouldBe(errorMessages);
    }

    public static void ShouldBeError(this Result result)
    {
        result.Status.ShouldBe(ResultStatus.Error);
    }

    public static void ShouldBeError(this Result result, string errorMessage)
    {
        result.ShouldBeError();
        result.Errors.ShouldBe(new[] { errorMessage });
    }

    public static void ShouldBeError(this Result result, ErrorList errorList)
    {
        result.ShouldBeError();
        result.Errors.ShouldBe(errorList.ErrorMessages);
        result.CorrelationId.ShouldBe(errorList.CorrelationId);
    }

    public static void ShouldBeError(
        this Result result,
        IEnumerable<string> errorMessages,
        string? correlationId)
    {
        result.ShouldBeError();
        result.Errors.ShouldBe(errorMessages);
        result.CorrelationId.ShouldBe(correlationId);
    }

    public static void ShouldBeConflict(this Result result)
    {
        result.Status.ShouldBe(ResultStatus.Conflict);
    }

    public static void ShouldBeConflict(this Result result, params string[] errorMessages)
    {
        result.ShouldBeConflict();
        result.Errors.ShouldBe(errorMessages);
    }

    public static void ShouldBeCriticalError(this Result result)
    {
        result.Status.ShouldBe(ResultStatus.CriticalError);
    }

    public static void ShouldBeCriticalError(this Result result, params string[] errorMessages)
    {
        result.ShouldBeCriticalError();
        result.Errors.ShouldBe(errorMessages);
    }

    public static void ShouldBeForbidden(this Result result)
    {
        result.Status.ShouldBe(ResultStatus.Forbidden);
    }

    public static void ShouldBeForbidden(this Result result, params string[] errorMessages)
    {
        result.ShouldBeForbidden();
        result.Errors.ShouldBe(errorMessages);
    }

    public static void ShouldBeNotFound(this Result result)
    {
        result.Status.ShouldBe(ResultStatus.NotFound);
    }

    public static void ShouldBeNotFound(this Result result, params string[] errorMessages)
    {
        result.ShouldBeNotFound();
        result.Errors.ShouldBe(errorMessages);
    }

    public static void ShouldBeInvalid(this Result result)
    {
        result.Status.ShouldBe(ResultStatus.Invalid);
    }

    public static void ShouldBeInvalid(
        this Result result,
        params ValidationError[] validationErrors)
    {
        result.ShouldBeInvalid();
        result.ValidationErrors.ShouldBe(validationErrors);
    }

    public static void ShouldHaveValidationErrorWithCode(
        this Result result,
        string errorCode)
    {
        result.ShouldBeInvalid();
        result.ValidationErrors.ShouldContain(x => x.ErrorCode == errorCode);
    }

    public static void ShouldHaveValidationErrorWithIdentifier(
        this Result result,
        string identifier)
    {
        result.ShouldBeInvalid();
        result.ValidationErrors.ShouldContain(x => x.Identifier == identifier);
    }

    public static void ShouldHaveValidationErrorWithMessage(
        this Result result,
        string errorMessage)
    {
        result.ShouldBeInvalid();
        result.ValidationErrors.ShouldContain(x => x.ErrorMessage == errorMessage);
    }

    public static void ShouldHaveValidationErrorWithSeverity(
        this Result result,
        ValidationSeverity severity)
    {
        result.ShouldBeInvalid();
        result.ValidationErrors.ShouldContain(x => x.Severity == severity);
    }

    public static void ShouldBeUnauthorized(this Result result)
    {
        result.Status.ShouldBe(ResultStatus.Unauthorized);
    }

    public static void ShouldBeUnauthorized(
        this Result result,
        params string[] errorMessages)
    {
        result.ShouldBeUnauthorized();
        result.Errors.ShouldBe(errorMessages);
    }

    public static void ShouldBeUnavailable(this Result result)
    {
        result.Status.ShouldBe(ResultStatus.Unavailable);
    }

    public static void ShouldBeUnavailable(
        this Result result,
        params string[] errorMessages)
    {
        result.ShouldBeUnavailable();
        result.Errors.ShouldBe(errorMessages);
    }

    public static void ShouldBeSuccess(this Result result)
    {
        result.IsSuccess.ShouldBeTrue();
    }

    public static void ShouldBeSuccess<T>(this Result<T> result)
    {
        result.IsSuccess.ShouldBeTrue();
    }

    public static void ShouldBeSuccessWithMessage(
        this Result result,
        string successMessage)
    {
        result.ShouldBeSuccess();
        result.SuccessMessage.ShouldBe(successMessage);
    }

    public static void ShouldBeCreatedWithLocation<T>(
        this Result<T> result,
        string location)
    {
        result.Status.ShouldBe(ResultStatus.Created);
        result.Location.ShouldBe(location);
    }
}