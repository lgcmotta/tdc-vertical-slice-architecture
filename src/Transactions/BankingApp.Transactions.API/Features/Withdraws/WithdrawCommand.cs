using BankingApp.Domain.Core;
using MediatR;

namespace BankingApp.Transactions.API.Features.Withdraws;

public record WithdrawCommand(string Token, decimal Amount) : IRequest<WithdrawTransactionResponse>, ICommand;