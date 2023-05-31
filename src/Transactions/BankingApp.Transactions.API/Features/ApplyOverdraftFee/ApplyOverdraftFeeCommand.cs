using BankingApp.Domain.Core;
using MediatR;

namespace BankingApp.Transactions.API.Features.ApplyOverdraftFee;

public record ApplyOverdraftFeeCommand(Guid HolderId, decimal OverdraftFee) : IRequest, ICommand;