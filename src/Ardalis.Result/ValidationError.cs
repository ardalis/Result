namespace Ardalis.Result
{
    public class ValidationError
    {
        public ValidationError(string identifier, string errorMessage, ValidationSeverity severity)
            : this(identifier, errorMessage)
        {
            Severity = severity;
        }
        public ValidationError(string identifier, string errorMessage)
        {
            Identifier = identifier;
            ErrorMessage = errorMessage;
        }
        public string Identifier { get; private set; }
        public string ErrorMessage { get; private set; }
        public ValidationSeverity Severity { get; private set; } = ValidationSeverity.Error;
    }
}
