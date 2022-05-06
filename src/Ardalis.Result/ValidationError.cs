namespace Ardalis.Result
{
    public class ValidationError
    {
        public string Identifier { get; set; }
        public string ErrorMessage { get; set; }
        public string ErrorCode { get; set; }
        public ValidationSeverity Severity { get; set; } = ValidationSeverity.Error;
    }
}
