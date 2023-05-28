using BankingApp.Domain.Core;
using MediatR;

namespace BankingApp.Transactions.API.Features.AccountUpdated;

public record UpdateAccountCommand(Guid HolderId, string? Name, string? Token, string? Currency) : IRequest, ICommand;