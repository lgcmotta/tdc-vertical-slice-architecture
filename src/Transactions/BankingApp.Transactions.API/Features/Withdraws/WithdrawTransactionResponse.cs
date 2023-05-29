namespace BankingApp.Transactions.API.Features.Withdraws;

public record WithdrawTransactionResponse(Guid TransactionId, string Type, string Currency, string FormattedAmount, decimal Amount);