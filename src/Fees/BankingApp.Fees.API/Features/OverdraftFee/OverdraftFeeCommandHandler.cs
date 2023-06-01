using BankingApp.Fees.API.Infrastructure;
using BankingApp.Fees.Domain.Entities;
using BankingApp.Fees.Domain.Events;
using BankingApp.Fees.Domain.ValueObjects;
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