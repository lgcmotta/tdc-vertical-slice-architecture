using BankingApp.Accounts.API.Infrastructure;
using BankingApp.Accounts.Domain.Exceptions;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace BankingApp.Accounts.API.Features.RetrieveTokenHistory;

public class RetrieveTokenHistoryQueryHandler : IRequestHandler<RetrieveTokenHistoryQuery, IEnumerable<RetrieveTokenHistoryResponse>>
{
    private readonly AccountHoldersDbContext _context;

    public RetrieveTokenHistoryQueryHandler(AccountHoldersDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<RetrieveTokenHistoryResponse>> Handle(RetrieveTokenHistoryQuery request, CancellationToken cancellationToken)
    {
        var account = await _context.Accounts
            .Include(account => account.Tokens)
            .FirstOrDefaultAsync(account => account.Tokens.Any(token => token.Value == request.Token && token.Enabled), cancellationToken)
            .ConfigureAwait(continueOnCapturedContext: false);

        if (account is null)
        {
            throw new AccountNotFoundException($"Account not found for token {request.Token}. Token might be disabled.");
        }

        return account.Tokens
            .Select(token => new RetrieveTokenHistoryResponse(token.Value, token.Enabled, token.CreatedAt, token.DisabledAt))
            .OrderBy(token => token.CreatedAt);
    }
}