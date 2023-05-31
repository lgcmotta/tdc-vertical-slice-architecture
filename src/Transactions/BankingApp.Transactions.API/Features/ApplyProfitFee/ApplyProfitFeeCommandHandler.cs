using BankingApp.Transactions.API.Infrastructure;
using BankingApp.Transactions.Domain.Exceptions;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace BankingApp.Transactions.API.Features.ApplyProfitFee;

public class ApplyProfitFeeCommandHandler : IRequestHandler<ApplyProfitFeeCommand>
{
    private readonly AccountsDbContext _context;

    public ApplyProfitFeeCommandHandler(AccountsDbContext context)
    {
        _context = context;
    }

    public async Task Handle(ApplyProfitFeeCommand request, CancellationToken cancellationToken)
    {
        var account = await _context.Accounts
            .FirstOrDefaultAsync(account => account.Id == request.HolderId, cancellationToken)
            .ConfigureAwait(continueOnCapturedContext: false);

        if (account is null)
        {
            throw new AccountNotFoundException($"Account not found for account holder {request.HolderId}");
        }

        account.ApplyProfitFee(request.Earnings, DateTime.UtcNow);
    }
}