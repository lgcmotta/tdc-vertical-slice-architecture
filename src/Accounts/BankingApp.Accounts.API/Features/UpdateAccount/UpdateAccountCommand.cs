using BankingApp.Domain.Core;
using MediatR;

namespace BankingApp.Accounts.API.Features.UpdateAccount;

public record UpdateAccountCommand(string Token, string FirstName, string LastName, string Document, string Currency) : IRequest, ICommand;