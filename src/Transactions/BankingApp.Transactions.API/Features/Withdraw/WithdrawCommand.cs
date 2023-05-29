using BankingApp.Domain.Core;
using MediatR;

namespace BankingApp.Transactions.API.Features.Withdraw;

public record WithdrawCommand(string Token, decimal Amount) : IRequest<WithdrawTransactionResponse>, ICommand;