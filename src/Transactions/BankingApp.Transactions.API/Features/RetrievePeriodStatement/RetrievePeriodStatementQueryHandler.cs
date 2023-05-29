using BankingApp.Transactions.API.Infrastructure;
using BankingApp.Transactions.Domain.Exceptions;
using BankingApp.Transactions.Domain.ValueObjects;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace BankingApp.Transactions.API.Features.RetrievePeriodStatement;

public class RetrievePeriodStatementQueryHandler : IRequestHandler<RetrievePeriodStatementQuery, IEnumerable<RetrievePeriodStatementModel>>
{
    private readonly AccountsDbContext _context;

    public RetrievePeriodStatementQueryHandler(AccountsDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<RetrievePeriodStatementModel>> Handle(RetrievePeriodStatementQuery request, CancellationToken cancellationToken)
    {
        var (start, end) = GetStatementPeriod(request.Start, request.End);

        var account = await _context.Accounts
            .Where(account => account.Token == request.Token)
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

            return new RetrievePeriodStatementModel
            {
                TransactionId = transaction.Id,
                Value = value,
                FormattedValue = value.Format(account.DisplayCurrency),
                PreviousBalance = previousBalance,
                FormattedPreviousBalance = previousBalance.Format(account.DisplayCurrency),
                Type = transaction.Type.Value,
                SenderToken = sender!.SenderToken,
                SenderName = sender.SenderName,
                ReceiverToken = receiver!.ReceiverToken,
                ReceiverName = receiver.ReceiverName,
                Occurrence = transaction.Occurence
            };
        });

        return periodStatement.OrderBy(statement => statement.Occurrence);
    }

    private static (DateTime Start, DateTime End) GetStatementPeriod(DateOnly? requestStart, DateOnly? requestEnd)
    {
        var start = requestStart?.ToDateTime(new TimeOnly(0, 0, 0, 0, 0)) ?? GetStartOfDay();

        var end = requestEnd?.ToDateTime(new TimeOnly(23, 59, 59, 999, 999)) ?? (requestStart is null
            ? GetEndOfDay(start)
            : GetEndOfDay(GetStartOfDay()));

        return (start, end);
    }

    private static DateTime GetStartOfDay()
    {
        var utcNow = DateTime.UtcNow;
        return new DateTime(utcNow.Year, utcNow.Month, utcNow.Day, 0, 0, 0, 0);
    }

    private static DateTime GetEndOfDay(DateTime startOfMonth)
    {
        return startOfMonth.AddMonths(1).AddDays(-1);
    }
}