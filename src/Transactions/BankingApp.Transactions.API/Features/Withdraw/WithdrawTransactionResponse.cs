namespace BankingApp.Transactions.API.Features.Withdraw;

public record WithdrawTransactionResponse(Guid TransactionId, string Type, string Currency, string FormattedAmount, decimal Amount);