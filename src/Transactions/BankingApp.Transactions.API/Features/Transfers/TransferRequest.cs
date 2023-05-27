namespace BankingApp.Transactions.API.Features.Transfers;

public record TransferRequest(decimal Amount, string SenderToken, string ReceiverToken);