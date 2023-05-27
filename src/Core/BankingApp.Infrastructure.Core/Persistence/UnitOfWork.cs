using BankingApp.Domain.Core;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

// ReSharper disable PossibleMultipleEnumeration
namespace BankingApp.Infrastructure.Core.Persistence;

public class UnitOfWork<TDbContext> : IUnitOfWork
    where TDbContext : DbContext
{
    private readonly TDbContext _context;

    public UnitOfWork(TDbContext context)
    {
        _context = context;
    }

    public IExecutionStrategy CreateExecutionStrategy()
    {
        return _context.Database.CreateExecutionStrategy();
    }

    public IEnumerable<INotification> ExtractDomainEventsFromAggregates()
    {
        var aggregates = _context.ChangeTracker
            .Entries<IAggregateRoot>()
            .Where(entityEntry => entityEntry.Entity.DomainEvents.Any());

        var domainEvents = aggregates.SelectMany(entityEntry => entityEntry.Entity.DomainEvents).ToArray();

        foreach (var aggregateEntry in aggregates)
        {
            aggregateEntry.Entity.ClearDomainEvents();
        }

        return domainEvents;
    }

    public async Task<IDbContextTransaction> BeginTransactionAsync(CancellationToken cancellationToken = default)
    {
        var transaction = await _context.Database.BeginTransactionAsync(cancellationToken)
            .ConfigureAwait(continueOnCapturedContext: false);

        return transaction;
    }

    public async Task CommitTransactionAsync(CancellationToken cancellationToken = default)
    {
        await _context.Database.CommitTransactionAsync(cancellationToken)
            .ConfigureAwait(continueOnCapturedContext: false);
    }

    public async Task RollBackTransactionAsync(CancellationToken cancellationToken = default)
    {
        await _context.Database.RollbackTransactionAsync(cancellationToken)
            .ConfigureAwait(continueOnCapturedContext: false);
    }

    public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        return await _context.SaveChangesAsync(cancellationToken)
            .ConfigureAwait(continueOnCapturedContext: false);
    }
}