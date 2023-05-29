using BankingApp.Transactions.API.Infrastructure;
using BankingApp.Transactions.Domain.Exceptions;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace BankingApp.Transactions.API.Features.ApplyEarnings;

public class ApplyEarningsCommandHandler : IRequestHandler<ApplyEarningsCommand>
{
    private readonly AccountsDbContext _context;

    public ApplyEarningsCommandHandler(AccountsDbContext context)
    {
        _context = context;
    }

    public async Task Handle(ApplyEarningsCommand request, CancellationToken cancellationToken)
    {
        var account = await _context.Accounts
            .FirstOrDefaultAsync(account => account.Id == request.HolderId, cancellationToken)
            .ConfigureAwait(continueOnCapturedContext: false);

        if (account is null)
        {
            throw new AccountNotFoundException($"Account not found for account holder {request.HolderId}");
        }

        account.ApplyEarnings(request.Earnings, DateTime.UtcNow);
    }
}