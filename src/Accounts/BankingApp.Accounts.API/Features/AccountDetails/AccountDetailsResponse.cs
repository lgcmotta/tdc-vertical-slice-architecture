namespace BankingApp.Accounts.API.Features.AccountDetails;

public record AccountDetailsResponse(string FirstName, string LastName, string Document, string Currency, DateTime CreatedAt, DateTime? ModifiedAt);