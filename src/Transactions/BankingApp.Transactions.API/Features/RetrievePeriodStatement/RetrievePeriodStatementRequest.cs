namespace BankingApp.Transactions.API.Features.RetrievePeriodStatement;

public record RetrievePeriodStatementRequest(string Token, DateOnly Start, DateOnly End);