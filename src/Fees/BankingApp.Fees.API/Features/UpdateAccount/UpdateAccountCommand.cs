using BankingApp.Domain.Core;
using MediatR;

namespace BankingApp.Fees.API.Features.UpdateAccount;

public record UpdateAccountCommand(Guid HolderId, string? Token) : IRequest, ICommand;