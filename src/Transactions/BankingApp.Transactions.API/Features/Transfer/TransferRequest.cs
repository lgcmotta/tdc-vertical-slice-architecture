namespace BankingApp.Transactions.API.Features.Transfer;

public record TransferRequest(decimal Amount, string Currency, string SenderToken, string ReceiverToken);