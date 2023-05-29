namespace BankingApp.Accounts.API.Features.TokenHistory;

public record TokenHistoryResponse(string Token, bool Enabled, DateTime CreatedAt, DateTime? DisabledAt);