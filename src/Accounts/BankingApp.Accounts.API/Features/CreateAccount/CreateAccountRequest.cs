// ReSharper disable ClassNeverInstantiated.Global
namespace BankingApp.Accounts.API.Features.CreateAccount;

public record CreateAccountRequest(string FirstName, string LastName, string Document, string Currency);