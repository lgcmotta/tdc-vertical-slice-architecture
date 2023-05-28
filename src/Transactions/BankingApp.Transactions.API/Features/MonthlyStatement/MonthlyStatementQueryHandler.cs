using BankingApp.Transactions.API.Infrastructure;
using BankingApp.Transactions.Domain.Entities;
using BankingApp.Transactions.Domain.Exceptions;
using BankingApp.Transactions.Domain.ValueObjects;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace BankingApp.Transactions.API.Features.MonthlyStatement;

public class MonthlyStatementQueryHandler : IRequestHandler<MonthlyStatementQuery, IEnumerable<MonthlyStatementModel>>
{
    private readonly AccountsDbContext _context;

    public MonthlyStatementQueryHandler(AccountsDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<MonthlyStatementModel>> Handle(MonthlyStatementQuery request, CancellationToken cancellationToken)
    {
        var (start, end) = GetMonthlyStatementPeriod(request.Year, request.Month);

        var account = await _context.Accounts
            .Include(account => Enumerable.Where<Transaction>(account.Transactions, t => t.Occurence >= start && t.Occurence <= end))
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

            return new MonthlyStatementModel
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

    private static (DateTime Start, DateTime End) GetMonthlyStatementPeriod(int year, int month)
    {
        var start = GetStartOfMonth(year, month);

        var end = GetEndOfMonth(start);

        return (start, end);
    }

    private static DateTime GetStartOfMonth(int year, int month)
    {
        return new DateTime(year, month, 1, 0, 0, 0, 0);
    }

    private static DateTime GetEndOfMonth(DateTime startOfMonth)
    {
        return startOfMonth.AddMonths(1).AddDays(-1);
    }
}