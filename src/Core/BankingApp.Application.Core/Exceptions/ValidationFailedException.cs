namespace BankingApp.Application.Core.Exceptions;

public class ValidationFailedException : Exception
{
    public ValidationFailedException(IEnumerable<ValidationError> errors)
    {
        Errors = errors;
    }

    public record ValidationError(string Property, string ErrorMessage);

    public IEnumerable<ValidationError> Errors { get; private set; }
}