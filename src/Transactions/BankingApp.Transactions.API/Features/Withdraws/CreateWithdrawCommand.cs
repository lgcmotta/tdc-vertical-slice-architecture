using MediatR;

namespace BankingApp.Transactions.API.Features.Withdraws;

public record CreateWithdrawCommand(string Token, decimal Amount) : IRequest<WithdrawTransactionResponse>;