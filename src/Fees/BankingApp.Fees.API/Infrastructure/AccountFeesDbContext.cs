using BankingApp.Fees.Domain;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace BankingApp.Fees.API.Infrastructure;

public sealed class AccountFeesDbContext : DbContext
{
    public AccountFeesDbContext(DbContextOptions<AccountFeesDbContext> options) : base(options)
    {
        ChangeTracker.LazyLoadingEnabled = false;
        ChangeTracker.AutoDetectChangesEnabled = true;
        ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.TrackAll;
    }

    public DbSet<Account> Accounts { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.Load("BankingApp.Infrastructure.Core"));
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(AccountFeesDbContext).Assembly);
    }
}