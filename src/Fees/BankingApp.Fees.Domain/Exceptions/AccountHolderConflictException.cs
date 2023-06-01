namespace BankingApp.Fees.Domain.Exceptions;

public class AccountHolderConflictException : Exception
{
    public AccountHolderConflictException(string? message) : base(message)
    { }
}