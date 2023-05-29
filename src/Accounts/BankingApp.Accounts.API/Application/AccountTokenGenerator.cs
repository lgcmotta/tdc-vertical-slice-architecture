using BankingApp.Accounts.Domain;

namespace BankingApp.Accounts.API.Application;

public class AccountTokenGenerator : IAccountTokenGenerator
{
    public string Generate() => Guid.NewGuid().ToString().Replace("-", string.Empty);
}