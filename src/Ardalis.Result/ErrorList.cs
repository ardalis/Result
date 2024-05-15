using System.Collections.Generic;

namespace Ardalis.Result;

/// <summary>
/// A wrapper class for a list of error messages and an optional CorrelationId.
/// </summary>
/// <param name="ErrorMessages"></param>
/// <param name="CorrelationId"></param>
public record ErrorList(IEnumerable<string> ErrorMessages, string? CorrelationId);
