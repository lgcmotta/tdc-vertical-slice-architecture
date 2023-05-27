using MediatR;

namespace BankingApp.Transactions.API.Features.Deposits;

public record DepositCommand(string Token, string Currency, decimal Amount) : IRequest<DepositTransactionResponse>;