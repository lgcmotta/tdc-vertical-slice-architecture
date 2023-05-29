namespace BankingApp.Accounts.API.Features.RetrieveAccountDetails;

public record RetrieveAccountDetailsResponse(string FirstName, string LastName, string Document, string Currency, DateTime CreatedAt, DateTime? ModifiedAt);