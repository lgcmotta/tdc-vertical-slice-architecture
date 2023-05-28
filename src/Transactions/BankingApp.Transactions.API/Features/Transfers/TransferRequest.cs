namespace BankingApp.Transactions.API.Features.Transfers;

public record TransferRequest(decimal Amount, string Currency, string SenderToken, string ReceiverToken);