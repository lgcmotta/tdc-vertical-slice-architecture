using BankingApp.Domain.Aggregates;
using BankingApp.Domain.UnitOfWork;
using BankingApp.Infrastructure.EntityFramework.Configurations;
using BankingApp.Infrastructure.Mediator;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Data;
using System.Threading;
using System.Threading.Tasks;

namespace BankingApp.Infrastructure.EntityFramework.DbContexts;

public class AccountsDbContext : DbContext, IDbContext, IUnitOfWork 
{
    private readonly IMediator _mediator;

    public bool HasActiveTransaction => CurrentTransaction is not null;

    public IDbContextTransaction CurrentTransaction { get; private set; }

    public virtual DbSet<Account> Accounts { get; set; }
        
    public AccountsDbContext(DbContextOptions<AccountsDbContext> dbContextOptions, IMediator mediator) : base(dbContextOptions)
    {
        _mediator = mediator;
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new AccountEntityTypeConfiguration());

        modelBuilder.ApplyConfiguration(new AccountBalanceEntityTypeConfiguration());
            
        modelBuilder.ApplyConfiguration(new AccountTransactionEntityTypeConfiguration());
    }

    private async Task DisposeCurrentTransactionAsync()
    {
        if (CurrentTransaction is null) return;

        await CurrentTransaction.DisposeAsync();

        CurrentTransaction = null;
    }

    public async Task<IDbContextTransaction> BeginTransactionAsync(CancellationToken cancellationToken = default)
    {
        if (CurrentTransaction is not null) return null;

        CurrentTransaction = await Database.BeginTransactionAsync(IsolationLevel.ReadCommitted, cancellationToken);

        return CurrentTransaction;
    }

    public async Task CommitTransactionAsync(IDbContextTransaction transaction, CancellationToken cancellationToken = default)
    {
        if (transaction is null) throw new ArgumentNullException(nameof(transaction));

        if (transaction != CurrentTransaction)
            throw new InvalidOperationException($"{transaction.TransactionId} is not the current transaction.");

        try
        {
            await SaveChangesAsync(cancellationToken);

            await transaction.CommitAsync(cancellationToken);

            await _mediator.DispatchDomainEventsAsync(this);
        }
        catch
        {
            await RollbackTransactionAsync(cancellationToken);

            throw;

        }
        finally
        {
            await DisposeCurrentTransactionAsync();
        }
    }
        
    public async Task RollbackTransactionAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            CurrentTransaction?.RollbackAsync(cancellationToken);
        }
        finally
        {
            await DisposeCurrentTransactionAsync();
        }
    }

    public async Task<bool> SaveEntitiesAsync(CancellationToken cancellationToken = default)
    {
        var affectedRows = await SaveChangesAsync(cancellationToken);

        return affectedRows > 0;
    }
}