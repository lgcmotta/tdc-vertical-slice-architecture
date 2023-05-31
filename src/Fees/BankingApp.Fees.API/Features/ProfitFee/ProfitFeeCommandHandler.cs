using BankingApp.Fees.API.Infrastructure;
using BankingApp.Taxes.Domain.Entities;
using BankingApp.Taxes.Domain.Events;
using BankingApp.Taxes.Domain.ValueObjects;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace BankingApp.Fees.API.Features.ProfitFee;

public class ProfitFeeCommandHandler : IRequestHandler<ProfitFeeCommand>
{
    private readonly AccountFeesDbContext _context;

    public ProfitFeeCommandHandler(AccountFeesDbContext context)
    {
        _context = context;
    }

    public async Task Handle(ProfitFeeCommand request, CancellationToken cancellationToken)
    {
        var balanceIdleMinDate = DateTime.UtcNow.AddMinutes(request.BalanceIdleInMinutes);

        var accounts = await _context.Accounts
            .Where(account => account.CurrentBalanceInUSD > Money.Zero && account.LastBalanceChange <= balanceIdleMinDate)
            .ToListAsync(cancellationToken)
            .ConfigureAwait(continueOnCapturedContext: false);

        foreach (var account in accounts)
        {
            var feeAmount = account.CurrentBalanceInUSD * request.Rate;
            account.CurrentBalanceInUSD += feeAmount;
            account.FeeHistory.Add(new FeeHistory
            {
                Amount = feeAmount,
                Type = FeeType.Profit,
                CreatedAt = DateTime.Now
            });

            account.AddDomainEvent(new ProfitFeeSettledDomainEvent(account.Id, feeAmount));
        }
    }
}