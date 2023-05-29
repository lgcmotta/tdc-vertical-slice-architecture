namespace BankingApp.Accounts.API.Features.RetrieveTokenHistory;

public record RetrieveTokenHistoryResponse(string Token, bool Enabled, DateTime CreatedAt, DateTime? DisabledAt);