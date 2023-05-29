using BankingApp.Accounts.API.Infrastructure;
using BankingApp.Accounts.Domain.Exceptions;
using BankingApp.Accounts.Domain.ValueObjects;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace BankingApp.Accounts.API.Features.UpdateAccountPartially;

public class UpdateAccountPartiallyCommandHandler : IRequestHandler<UpdateAccountPartiallyCommand>
{
    private readonly AccountHoldersDbContext _context;

    public UpdateAccountPartiallyCommandHandler(AccountHoldersDbContext context)
    {
        _context = context;
    }

    public async Task Handle(UpdateAccountPartiallyCommand request, CancellationToken cancellationToken)
    {
        var account = await _context.Accounts
            .Include(account => account.Tokens.Where(token => token.Enabled))
            .FirstOrDefaultAsync(account => account.Tokens.Any(token => token.Value == request.Token && token.Enabled), cancellationToken)
            .ConfigureAwait(continueOnCapturedContext: false);

        if (account is null)
        {
            throw new AccountNotFoundException($"Account not found for token {request.Token}. Token might be disabled.");
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

        account.AddAccountPartiallyUpdatedDomainEvent();
    }
}