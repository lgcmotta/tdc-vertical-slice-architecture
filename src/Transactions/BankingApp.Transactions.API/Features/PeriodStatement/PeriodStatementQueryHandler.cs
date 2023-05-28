using BankingApp.Transactions.API.Infrastructure;
using BankingApp.Transactions.Domain.Exceptions;
using BankingApp.Transactions.Domain.ValueObjects;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace BankingApp.Transactions.API.Features.PeriodStatement;

public class PeriodStatementQueryHandler : IRequestHandler<PeriodStatementQuery, IEnumerable<PeriodStatementModel>>
{
    private readonly AccountsDbContext _context;

    public PeriodStatementQueryHandler(AccountsDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<PeriodStatementModel>> Handle(PeriodStatementQuery request, CancellationToken cancellationToken)
    {
        var (start, end) = GetStatementPeriod(request.Start, request.End);

        var account = await _context.Accounts
            .Include(account => account.Transactions.Where(t => t.Occurence >= start && t.Occurence <= end))
            .FirstOrDefaultAsync(cancellationToken)
            .ConfigureAwait(continueOnCapturedContext: false);

        if (account is null)
        {
            throw new AccountNotFoundException($"Account not found for token {request.Token}");
        }

        var senders = await _context.Accounts
            .Where(sender => account.Transactions.Select(transaction => transaction.SenderId).Contains(sender.Id))
            .Select(sender => new { SenderId = sender.Id, SenderName = sender.Name, SenderToken = sender.Token })
            .ToListAsync(cancellationToken)
            .ConfigureAwait(continueOnCapturedContext: false);

        var receivers = await _context.Accounts
            .Where(receiver => account.Transactions.Select(transaction => transaction.ReceiverId).Contains(receiver.Id))
            .Select(receiver => new { ReceiverId = receiver.Id, ReceiverName = receiver.Name, ReceiverToken = receiver.Token })
            .ToListAsync(cancellationToken)
            .ConfigureAwait(continueOnCapturedContext: false);

        var periodStatement = account.Transactions.Select(transaction =>
        {
            var value = Money.ConvertFromUSD(transaction.USDValue, account.DisplayCurrency);
            var previousBalance = Money.ConvertFromUSD(transaction.GetBalanceBeforeTransaction(), account.DisplayCurrency);
            var sender = senders.FirstOrDefault(sender => sender.SenderId == transaction.SenderId);
            var receiver = receivers.FirstOrDefault(receiver => receiver.ReceiverId == transaction.ReceiverId);

            return new PeriodStatementModel
            {
                TransactionId = transaction.Id,
                Value = value,
                FormattedValue = value.Format(account.DisplayCurrency),
                PreviousBalance = previousBalance,
                FormattedPreviousBalance = previousBalance.Format(account.DisplayCurrency),
                Type = transaction.Type.Value,
                SenderToken = sender!.SenderToken,
                SenderName = sender!.SenderName,
                ReceiverToken = receiver!.ReceiverToken,
                ReceiverName = receiver!.ReceiverName,
                Occurrence = transaction.Occurence
            };
        });

        return periodStatement.OrderBy(statement => statement.Occurrence);
    }

    private static (DateTime Start, DateTime End) GetStatementPeriod(DateOnly? requestStart, DateOnly? requestEnd)
    {
        var start = requestStart?.ToDateTime(new TimeOnly(0, 0, 0, 0, 0)) ?? GetStartOfMonth();

        var end = requestEnd?.ToDateTime(new TimeOnly(23, 59, 59, 999, 999)) ?? (requestStart is null
            ? GetEndOfMonth(start)
            : GetEndOfMonth(GetStartOfMonth()));

        return (start, end);
    }

    private static DateTime GetStartOfMonth()
    {
        var utcNow = DateTime.UtcNow;
        return new DateTime(utcNow.Year, utcNow.Month, 1, 0, 0, 0, 0);
    }

    private static DateTime GetEndOfMonth(DateTime startOfMonth)
    {
        return startOfMonth.AddMonths(1).AddDays(-1);
    }
}