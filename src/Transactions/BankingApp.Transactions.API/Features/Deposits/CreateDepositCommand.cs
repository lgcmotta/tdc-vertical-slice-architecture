using MediatR;

namespace BankingApp.Transactions.API.Features.Deposits;

public record CreateDepositCommand(string Token, string Currency, decimal Amount) : IRequest<DepositTransactionResponse>;