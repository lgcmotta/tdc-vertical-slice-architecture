namespace BankingApp.Accounts.Domain.Exceptions;

public class InvalidTransactionValueException : Exception
{
    public InvalidTransactionValueException(string? message) : base(message)
    { }
}