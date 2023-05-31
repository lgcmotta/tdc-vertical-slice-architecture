using BankingApp.Infrastructure.Core.Factories;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace BankingApp.Fees.API.Infrastructure;

public class AccountFeesDbContextDesignTimeFactory : IDesignTimeDbContextFactory<AccountFeesDbContext>
{
    public AccountFeesDbContext CreateDbContext(string[] args)
    {
        var configuration = ConfigurationFactory.CreateConfiguration();

        var connectionString = configuration.GetConnectionString(nameof(AccountFeesDbContext));

        if (string.IsNullOrWhiteSpace(connectionString))
        {
            throw new Exception("Connection string not found.");
        }

        var builder = new DbContextOptionsBuilder<AccountFeesDbContext>()
            .UseMySql(connectionString, ServerVersion.AutoDetect(connectionString));

        return new AccountFeesDbContext(builder.Options);
    }
}