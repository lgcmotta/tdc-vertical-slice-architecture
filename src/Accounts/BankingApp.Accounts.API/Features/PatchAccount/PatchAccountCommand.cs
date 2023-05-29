using BankingApp.Domain.Core;
using MediatR;

namespace BankingApp.Accounts.API.Features.PatchAccount;

public record PatchAccountCommand(string Token, string? FirstName, string? LastName, string? Document, string? Currency) : IRequest, ICommand;