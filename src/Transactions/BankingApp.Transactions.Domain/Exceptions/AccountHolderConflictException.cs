namespace BankingApp.Transactions.Domain.Exceptions;

public class AccountHolderConflictException : Exception
{
    public AccountHolderConflictException(string? message) : base(message)
    { }
}