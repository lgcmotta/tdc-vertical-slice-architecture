using BankingApp.Accounts.API.Infrastructure;
using BankingApp.Accounts.Domain.Entities;
using BankingApp.Accounts.Domain.Exceptions;
using BankingApp.Accounts.Domain.ValueObjects;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace BankingApp.Accounts.API.Features.PatchAccount;

public class PatchAccountCommandHandler : IRequestHandler<PatchAccountCommand>
{
    private readonly AccountHoldersDbContext _context;

    public PatchAccountCommandHandler(AccountHoldersDbContext context)
    {
        _context = context;
    }

    public async Task Handle(PatchAccountCommand request, CancellationToken cancellationToken)
    {
        var account = await _context.Accounts
            .Include(account => Enumerable.Where<AccountToken>(account.Tokens, token => token.Enabled))
            .FirstOrDefaultAsync(account => account.Tokens.Any(token => token.Value == request.Token), cancellationToken)
            .ConfigureAwait(continueOnCapturedContext: false);

        if (account is null)
        {
            throw new AccountNotFoundException($"Account not found for token {request.Token}");
        }

        if (!string.IsNullOrWhiteSpace(request.FirstName))
        {
            account.CorrectFirstName(request.FirstName);
        }

        if (!string.IsNullOrWhiteSpace(request.LastName))
        {
            account.CorrectLastName(request.LastName);
        }

        if (!string.IsNullOrWhiteSpace(request.Document))
        {
            account.ChangeDocument(request.Document);
        }

        if (!string.IsNullOrWhiteSpace(request.Currency) &&
            Currency.TryParseByValue<Currency>(request.Currency, out var currency))
        {
            account.ChangeCurrency(currency!);
        }

        account.AddAccountPatchedDomainEvent();
    }
}