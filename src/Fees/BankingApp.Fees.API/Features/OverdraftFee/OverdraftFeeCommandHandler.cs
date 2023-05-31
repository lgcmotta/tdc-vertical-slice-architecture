using BankingApp.Fees.API.Infrastructure;
using BankingApp.Taxes.Domain.Entities;
using BankingApp.Taxes.Domain.Events;
using BankingApp.Taxes.Domain.ValueObjects;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace BankingApp.Fees.API.Features.OverdraftFee;

public class OverdraftFeeCommandHandler : IRequestHandler<OverdraftFeeCommand>
{
    private readonly AccountFeesDbContext _context;

    public OverdraftFeeCommandHandler(AccountFeesDbContext context)
    {
        _context = context;
    }

    public async Task Handle(OverdraftFeeCommand request, CancellationToken cancellationToken)
    {
        var accounts = await _context.Accounts
            .Where(account => account.CurrentBalanceInUSD < Money.Zero)
            .ToListAsync(cancellationToken)
            .ConfigureAwait(continueOnCapturedContext: false);

        if (!accounts.Any()) return;

        foreach (var account in accounts)
        {
            var feeAmount = account.CurrentBalanceInUSD * request.Rate;
            account.CurrentBalanceInUSD += feeAmount;
            account.FeeHistory.Add(new FeeHistory
            {
                Amount = feeAmount,
                Type = FeeType.Overdraft,
                CreatedAt = DateTime.UtcNow
            });
            
            account.AddDomainEvent(new OverdraftFeeSettledDomainEvent(account.Id, feeAmount));
        }
    }
}