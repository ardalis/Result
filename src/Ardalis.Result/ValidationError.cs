namespace Ardalis.Result
{
    public class ValidationError
    {
        // TODO: Mark required and limit setting (see #179)
        public string Identifier { get; set; }
        // TODO: Mark required and limit setting (see #179)
        public string ErrorMessage { get; set; }
        public string ErrorCode { get; set; }
        public ValidationSeverity Severity { get; set; } = ValidationSeverity.Error;
    }
}
