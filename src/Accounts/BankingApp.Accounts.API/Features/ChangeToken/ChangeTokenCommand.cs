using BankingApp.Domain.Core;
using MediatR;

namespace BankingApp.Accounts.API.Features.ChangeToken;

public record ChangeTokenCommand(string Token) : IRequest<ChangeTokenResponse>, ICommand;