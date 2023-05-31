using BankingApp.Domain.Core;
using MediatR;

namespace BankingApp.Fees.API.Features.UpdateBalance;

public record UpdateBalanceCommand(Guid HolderId, decimal Balance) : IRequest, ICommand;