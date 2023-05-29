using BankingApp.Accounts.Domain;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace BankingApp.Accounts.API.Infrastructure;

public sealed class AccountHoldersDbContext : DbContext
{
    public AccountHoldersDbContext(DbContextOptions<AccountHoldersDbContext> options) : base(options)
    {
        ChangeTracker.LazyLoadingEnabled = false;
        ChangeTracker.AutoDetectChangesEnabled = true;
        ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.TrackAll;
    }

    public DbSet<AccountHolder> Accounts { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.Load("BankingApp.Infrastructure.Core"));
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(AccountHoldersDbContext).Assembly);
    }
}