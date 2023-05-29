using BankingApp.Domain.Core;
using MediatR;

namespace BankingApp.Accounts.API.Features.CreateAccount;

public record CreateAccountCommand(string FirstName, string LastName, string Document, string Currency) : IRequest<AccountCreatedResponse>, ICommand;