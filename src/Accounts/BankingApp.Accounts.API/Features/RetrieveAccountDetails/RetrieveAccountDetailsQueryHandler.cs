using BankingApp.Accounts.API.Infrastructure;
using BankingApp.Accounts.Domain.Exceptions;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace BankingApp.Accounts.API.Features.RetrieveAccountDetails;

public class RetrieveAccountDetailsQueryHandler : IRequestHandler<RetrieveAccountDetailsQuery, RetrieveAccountDetailsResponse>
{
    private readonly AccountHoldersDbContext _context;

    public RetrieveAccountDetailsQueryHandler(AccountHoldersDbContext context)
    {
        _context = context;
    }

    public async Task<RetrieveAccountDetailsResponse> Handle(RetrieveAccountDetailsQuery request, CancellationToken cancellationToken)
    {
        var account = await _context.Accounts
            .Include(account => account.Tokens.Where(token => token.Enabled))
            .FirstOrDefaultAsync(account => account.Tokens.Any(token => token.Value == request.Token && token.Enabled), cancellationToken)
            .ConfigureAwait(continueOnCapturedContext: false);

        if (account is null)
        {
            throw new AccountNotFoundException($"Account not found for token {request.Token}. Token might be disabled.");
        }

        return new RetrieveAccountDetailsResponse(
            account.FirstName,
            account.LastName,
            account.Document,
            account.Currency.Value,
            account.CreatedAt,
            account.ModifiedAt
        );
    }
}