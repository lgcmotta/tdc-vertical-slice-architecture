using BankingApp.Domain.Core;
using MediatR;

// ReSharper disable ClassNeverInstantiated.Global
namespace BankingApp.Transactions.API.Features.Transfers;

public record TransferCommand(decimal Amount, string SenderToken, string ReceiverToken) : IRequest<TransferTransactionResponse>, ICommand;