using MediatR;
using Microsoft.EntityFrameworkCore.Storage;

namespace BankingApp.Infrastructure.Core.Persistence;

public interface IUnitOfWork
{
    IExecutionStrategy CreateExecutionStrategy();
    IEnumerable<INotification> ExtractDomainEventsFromAggregates();
    Task<IDbContextTransaction> BeginTransactionAsync(CancellationToken cancellationToken = default);
    Task CommitTransactionAsync(CancellationToken cancellationToken = default);
    Task RollBackTransactionAsync(CancellationToken cancellationToken = default);
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}