using System;
using DesafioWarren.Infrastructure.Mediator;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace DesafioWarren.Infrastructure.EntityFramework.DbContexts
{
    public class AccountsDesignTimeDbContextFactory : IDesignTimeDbContextFactory<AccountsDbContext> 
    {
        public AccountsDbContext CreateDbContext(string[] args)
        {
            var configuration = new ConfigurationBuilder()
                .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appsettings{GetAspNetCoreEnvironmentVariable()}.json", optional: false
                    , reloadOnChange: true)
                .Build();

            var connectionString = configuration.GetConnectionString(nameof(AccountsDbContext));

            var dbContextOptionsBuilder = new DbContextOptionsBuilder<AccountsDbContext>().UseMySql(connectionString, ServerVersion.Parse("8.0.27"));

            return new AccountsDbContext(dbContextOptionsBuilder.Options, new FakeMediator());
        }

        private static string GetAspNetCoreEnvironmentVariable()
        {
            var aspnetCoreEnvironment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");

            return string.IsNullOrEmpty(aspnetCoreEnvironment)
                ? string.Empty
                : string.Concat(".", aspnetCoreEnvironment);
        }
    }
}