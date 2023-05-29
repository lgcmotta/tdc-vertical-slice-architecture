using BankingApp.Domain.Core;
using MediatR;

namespace BankingApp.Transactions.API.Features.CreateAccount;

public record CreateAccountCommand(Guid HolderId, string Name, string Document, string Token, string Currency) : IRequest, ICommand;