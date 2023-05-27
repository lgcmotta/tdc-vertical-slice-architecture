using BankingApp.Transactions.API.Infrastructure;
using BankingApp.Transactions.Domain.Exceptions;
using BankingApp.Transactions.Domain.ValueObjects;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

// ReSharper disable PossibleMultipleEnumeration
// ReSharper disable ClassNeverInstantiated.Local
// ReSharper disable ClassNeverInstantiated.Global
namespace BankingApp.Transactions.API.Features.PeriodStatement;

public record PeriodStatementRequest(string Token, DateOnly Start, DateOnly End);

public static class PeriodStatementEndpoint
{
    public static async Task<IResult> GetAsync(
        [FromServices] IMediator mediator,
        [FromRoute] string token,
        [FromQuery] DateOnly? start = null,
        [FromQuery] DateOnly? end = null,
        CancellationToken cancellationToken = default)
    {

        var query = new PeriodStatementQuery(token, start, end);

        var response = await mediator.Send(query, cancellationToken)
            .ConfigureAwait(continueOnCapturedContext: false);

        return response.Any() ? Results.Ok(response) : Results.NotFound();
    }
}

public record PeriodStatementResult(Guid TransactionId, decimal Value, decimal BalanceBefore, string Type, string SenderToken, string SenderName, string ReceiverToken, string ReceiverName, DateTime Occurrence);
file record AccountModel(Guid Id, HolderModel Holder, int Currency, IEnumerable<TransactionModel> Transactions);
file record HolderModel(Guid Id, Guid AccountId, string Name, string Document, string Token);
file record TransactionModel(Guid Id, Guid AccountId, Guid Sender, Guid Receiver, decimal Value, decimal BalanceSnapShot, int Type, DateTime Occurence);
public record PeriodStatementQuery(string Token, DateOnly? Start, DateOnly? End) : IRequest<IEnumerable<PeriodStatementResult>>;

public class PeriodStatementQueryHandler : IRequestHandler<PeriodStatementQuery, IEnumerable<PeriodStatementResult>>
{
    private readonly AccountsDbContext _context;

    public PeriodStatementQueryHandler(AccountsDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<PeriodStatementResult>> Handle(PeriodStatementQuery request, CancellationToken cancellationToken)
    {
        var (start, end) = GetStatementPeriod(request.Start, request.End);

        var accountExists = await _context.Set<AccountModel>()
            .AnyAsync(account => account.Holder.Token == request.Token, cancellationToken)
            .ConfigureAwait(continueOnCapturedContext: false);

        if (!accountExists)
        {
            throw new AccountNotFoundException($"Account not found for token {request.Token}");
        }

        var transactionsQueryable =  _context.Set<AccountModel>()
            .Where(account => account.Holder.Token == request.Token)
            .SelectMany(account => account.Transactions
                .Where(transaction => transaction.Occurence >= start && transaction.Occurence <= end)
                .Select(transaction => new
                {
                    Transaction = transaction,
                    Sender = _context.Set<AccountModel>().Include(sender => sender.Holder).FirstOrDefault(sender => sender.Id == transaction.Sender),
                    Receiver = _context.Set<AccountModel>().Include(receiver => receiver.Holder).FirstOrDefault(receiver => receiver.Id == transaction.Receiver),
                }));

        var statement = await transactionsQueryable
            .AsSingleQuery()
            .Select(result => new PeriodStatementResult(
                result.Transaction.Id,
                result.Transaction.Value,
                result.Transaction.BalanceSnapShot,
                TransactionType.ParseByKey<TransactionType>(result.Transaction.Type).Value,
                result.Sender!.Holder.Token,
                result.Sender.Holder.Name,
                result.Receiver!.Holder.Token,
                result.Receiver.Holder.Name,
                result.Transaction.Occurence))
            .ToListAsync(cancellationToken)
            .ConfigureAwait(continueOnCapturedContext: false);

        return statement;
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