using System.Collections.Generic;

namespace Ardalis.Result;

public record ErrorList(IEnumerable<string> ErrorMessages, string? CorrelationId);
