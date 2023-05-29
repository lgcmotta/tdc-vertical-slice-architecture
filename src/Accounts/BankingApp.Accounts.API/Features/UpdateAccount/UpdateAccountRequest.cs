namespace BankingApp.Accounts.API.Features.UpdateAccount;

public record UpdateAccountRequest(string Token, string FirstName, string LastName, string Document, string Currency);