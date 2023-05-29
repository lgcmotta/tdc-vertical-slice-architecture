using BankingApp.Domain.Core;
using MediatR;

namespace BankingApp.Accounts.API.Features.UpdateAccountPartially;

public record UpdateAccountPartiallyCommand(string Token, string? FirstName, string? LastName, string? Document, string? Currency) : IRequest, ICommand;