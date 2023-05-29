using BankingApp.Domain.Core;
using MediatR;

namespace BankingApp.Transactions.API.Features.Deposit;

public record DepositCommand(string Token, string Currency, decimal Amount) : IRequest<DepositTransactionResponse>, ICommand;