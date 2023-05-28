using BankingApp.Infrastructure.Core.Factories;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace BankingApp.Accounts.API.Infrastructure;

public class AccountHoldersDbContextDesignTimeFactory : IDesignTimeDbContextFactory<AccountHoldersDbContext>
{
    public AccountHoldersDbContext CreateDbContext(string[] args)
    {
        var configuration = ConfigurationFactory.CreateConfiguration();

        var connectionString = configuration.GetConnectionString(nameof(AccountHoldersDbContext));

        if (string.IsNullOrWhiteSpace(connectionString))
        {
            throw new Exception("Connection string not found.");
        }

        var builder = new DbContextOptionsBuilder<AccountHoldersDbContext>()
            .UseMySql(connectionString, ServerVersion.AutoDetect(connectionString));

        return new AccountHoldersDbContext(builder.Options);
    }
}