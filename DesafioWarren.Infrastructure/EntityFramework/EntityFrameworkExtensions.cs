using System;
using System.Threading.Tasks;
using DesafioWarren.Infrastructure.EntityFramework.DbContexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace DesafioWarren.Infrastructure.EntityFramework
{
    public static class EntityFrameworkExtensions
    {
        public static async Task<IServiceProvider> MigrateDbContextAsync(this IServiceProvider serviceProvider)
        {
            if (Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") == "Production")
                return serviceProvider;

            using var scope = serviceProvider.GetService<IServiceScopeFactory>()?.CreateScope();

            var dbContext = scope?.ServiceProvider.GetRequiredService<AccountsDbContext>();

            if (dbContext is not null)
                await dbContext.Database.MigrateAsync();

            return serviceProvider;
        }

        public static IServiceCollection AddAccountsDbContext(this IServiceCollection serviceCollection, IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString(nameof(AccountsDbContext));

            serviceCollection.AddDbContext<AccountsDbContext>(options =>
            {
                options.UseMySql(connectionString, ServerVersion.Parse("8.0.27"), mysqlOptions =>
                {
                    mysqlOptions.EnableRetryOnFailure(10, TimeSpan.FromSeconds(30), null);
                });
            });

            return serviceCollection;
        }
    }
}