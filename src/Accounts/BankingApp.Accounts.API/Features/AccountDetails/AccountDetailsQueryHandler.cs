using BankingApp.Accounts.API.Infrastructure;
using BankingApp.Accounts.Domain.Exceptions;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace BankingApp.Accounts.API.Features.AccountDetails;

public class AccountDetailsQueryHandler : IRequestHandler<AccountDetailsQuery, AccountDetailsResponse>
{
    private readonly AccountHoldersDbContext _context;

    public AccountDetailsQueryHandler(AccountHoldersDbContext context)
    {
        _context = context;
    }

    public async Task<AccountDetailsResponse> Handle(AccountDetailsQuery request, CancellationToken cancellationToken)
    {
        var account = await _context.Accounts
            .Include(account => account.Tokens.Where(token => token.Enabled))
            .FirstOrDefaultAsync(account => account.Tokens.Any(token => token.Value == request.Token && token.Enabled), cancellationToken)
            .ConfigureAwait(continueOnCapturedContext: false);

        if (account is null)
        {
            throw new AccountNotFoundException($"Account not found for token {request.Token}. Token might be disabled.");
        }

        return new AccountDetailsResponse(
            account.FirstName,
            account.LastName,
            account.Document,
            account.Currency.Value,
            account.CreatedAt,
            account.ModifiedAt
        );
    }
}