using BankingApp.Fees.API.Infrastructure;
using BankingApp.Taxes.Domain.Exceptions;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace BankingApp.Fees.API.Features.UpdateBalance;

public class UpdateBalanceCommandHandler : IRequestHandler<UpdateBalanceCommand>
{
    private readonly AccountFeesDbContext _context;

    public UpdateBalanceCommandHandler(AccountFeesDbContext context)
    {
        _context = context;
    }

    public async Task Handle(UpdateBalanceCommand request, CancellationToken cancellationToken)
    {
        var account = await _context.Accounts
            .FirstOrDefaultAsync(account => account.Id == request.HolderId, cancellationToken)
            .ConfigureAwait(continueOnCapturedContext: false);

        if (account is null)
        {
            throw new AccountNotFoundException($"Account not found for account holder {request.HolderId}");
        }

        account.CurrentBalanceInUSD = request.Balance;
        account.LastBalanceChange = DateTime.UtcNow;
    }
}