using BankingApp.Domain.Core;
using MediatR;

namespace BankingApp.Fees.API.Features.CreateAccount;

public record CreateAccountCommand(Guid HolderId, string Token) : IRequest, ICommand;