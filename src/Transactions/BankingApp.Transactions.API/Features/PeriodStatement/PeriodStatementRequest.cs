namespace BankingApp.Transactions.API.Features.PeriodStatement;

public record PeriodStatementRequest(string Token, DateOnly Start, DateOnly End);