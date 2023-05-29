namespace BankingApp.Accounts.Domain.Exceptions;

public class AccountHolderCurrentTokenNotFound : Exception
{
    public AccountHolderCurrentTokenNotFound(string? message) : base(message)
    { }
}