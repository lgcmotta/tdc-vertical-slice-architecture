using BankingApp.Domain.Core;
using MediatR;

namespace BankingApp.Transactions.API.Features.Transfers;

public record TransferCommand(decimal Amount, string Currency, string SenderToken, string ReceiverToken)
    : IRequest<TransferTransactionResponse>, ICommand;