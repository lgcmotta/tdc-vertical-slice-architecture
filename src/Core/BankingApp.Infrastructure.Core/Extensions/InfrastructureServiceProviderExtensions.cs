using BankingApp.Infrastructure.Core.Scanners;
using EFCoreSecondLevelCacheInterceptor;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

// ReSharper disable PossibleMultipleEnumeration
namespace BankingApp.Infrastructure.Core.Extensions;

public static class InfrastructureServiceProviderExtensions
{
    public static IEnumerable<IInterceptor> ResolveEfCoreInterceptors(
        this IServiceProvider serviceProvider,
        bool enableSecondLevelCache,
        params Assembly[] interceptorsAssemblies)
    {
        var scannedInterceptors = InterceptorAssemblyScanner.Scan(serviceProvider, interceptorsAssemblies);

        return enableSecondLevelCache switch
        {
            true when !scannedInterceptors.Any() => new[] { (IInterceptor)serviceProvider.GetRequiredService<SecondLevelCacheInterceptor>() },
            true => scannedInterceptors.Concat(new[] { (IInterceptor)serviceProvider.GetRequiredService<SecondLevelCacheInterceptor>() }),
            false when !scannedInterceptors.Any() => Enumerable.Empty<IInterceptor>(),
            false => scannedInterceptors
        };
    }

    public static async Task ApplyMigrationsAsync<TDbContext>(
        this IServiceProvider serviceProvider,
        CancellationToken cancellationToken = default)
        where TDbContext : DbContext
    {
        using var scope = serviceProvider.CreateScope();
        var context = scope.ServiceProvider.GetService<TDbContext>();

        if (context is null)
        {
            throw new InvalidOperationException($"{nameof(TDbContext)} cannot be resolved.");
        }

        await context.Database.MigrateAsync(cancellationToken);
    }
}