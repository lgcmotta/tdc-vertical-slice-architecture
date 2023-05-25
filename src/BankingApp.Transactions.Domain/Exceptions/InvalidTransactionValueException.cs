namespace BankingApp.Transactions.Domain.Exceptions;

public class InvalidTransactionValueException : Exception
{
    public InvalidTransactionValueException(string? message) : base(message)
    { }
}