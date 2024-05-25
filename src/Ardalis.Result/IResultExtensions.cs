namespace Ardalis.Result;

public static class IResultExtensions
{
    /// <summary>
    /// Returns true if the result is successful (status is Ok).
    /// </summary>
    public static bool IsOk(this IResult result) => result.Status == ResultStatus.Ok;

    /// <summary>
    /// Returns true if the result is created (status is Created).
    /// </summary>
    public static bool IsCreated(this IResult result) => result.Status == ResultStatus.Created;

    /// <summary>
    /// Returns true if the result is an error (status is Error).
    /// </summary>
    public static bool IsError(this IResult result) => result.Status == ResultStatus.Error;

    /// <summary>
    /// Returns true if the result is forbidden (status is Forbidden).
    /// </summary>
    public static bool IsForbidden(this IResult result) => result.Status == ResultStatus.Forbidden;

    /// <summary>
    /// Returns true if the result is unauthorized (status is Unauthorized).
    /// </summary>
    public static bool IsUnauthorized(this IResult result) => result.Status == ResultStatus.Unauthorized;

    /// <summary>
    /// Returns true if the result is invalid (status is Invalid).
    /// </summary>
    public static bool IsInvalid(this IResult result) => result.Status == ResultStatus.Invalid;

    /// <summary>
    /// Returns true if the result is not found (status is NotFound).
    /// </summary>
    public static bool IsNotFound(this IResult result) => result.Status == ResultStatus.NotFound;

    /// <summary>
    /// Returns true if the result is no content (status is NoContent).
    /// </summary>
    public static bool IsNoContent(this IResult result) => result.Status == ResultStatus.NoContent;

    /// <summary>
    /// Returns true if the result is a conflict (status is Conflict).
    /// </summary>
    public static bool IsConflict(this IResult result) => result.Status == ResultStatus.Conflict;

    /// <summary>
    /// Returns true if the result is a critical error (status is CriticalError).
    /// </summary>
    public static bool IsCriticalError(this IResult result) => result.Status == ResultStatus.CriticalError;

    /// <summary>
    /// Returns true if the result is unavailable (status is Unavailable).
    /// </summary>
    public static bool IsUnavailable(this IResult result) => result.Status == ResultStatus.Unavailable;
}
