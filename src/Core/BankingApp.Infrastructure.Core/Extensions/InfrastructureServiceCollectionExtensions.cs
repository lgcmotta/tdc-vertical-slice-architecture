using EFCoreSecondLevelCacheInterceptor;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

// ReSharper disable PossibleMultipleEnumeration
namespace BankingApp.Infrastructure.Core.Extensions;

public static class InfrastructureServiceCollectionExtensions
{
    public static IServiceCollection AddMySqlDbContext<TDbContext>(
        this IServiceCollection services,
        IConfiguration configuration,
        ServiceLifetime? serviceLifetime,
        params Assembly[] interceptorsAssemblies) where TDbContext : DbContext
    {
        var connectionString = configuration.GetConnectionString(nameof(TDbContext));

        if (string.IsNullOrWhiteSpace(connectionString))
        {
            throw new InvalidOperationException($"Connection string for {nameof(TDbContext)} was not found.");
        }

        var maxRetryCount = configuration.GetValue<int>("MySql:MaxRetryCount");
        var enableSecondLevelCache = configuration.GetValue<bool>("MySql:EnableSecondLevelCache");

        if (enableSecondLevelCache)
        {
            services.AddEFSecondLevelCache(options =>
                options.UseMemoryCacheProvider().DisableLogging(value: true)
            );
        }

        services.AddDbContext<TDbContext>((provider, optionsBuilder) =>
        {
            optionsBuilder.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString), mysqlBuilder =>
            {
                mysqlBuilder.EnableRetryOnFailure(maxRetryCount);

            });

            var interceptors = provider.ResolveEfCoreInterceptors(enableSecondLevelCache, interceptorsAssemblies);

            if (interceptors.Any())
            {
                optionsBuilder.AddInterceptors(interceptors);
            }
        }, serviceLifetime ?? ServiceLifetime.Scoped);

        return services;
    }

    public static IServiceCollection AddRabbitMqMessaging(this IServiceCollection services,
        IConfiguration configuration)
    {
        var host = configuration.GetValue<string>("RabbitMQ:Host");
        var virtualHost = configuration.GetValue<string>("RabbitMQ:VirtualHost") ?? "/";
        var username = configuration.GetValue<string>("RabbitMQ:Username") ?? "guest";
        var password = configuration.GetValue<string>("RabbitMQ:Password") ?? "guest";

        if (string.IsNullOrWhiteSpace(host))
        {
            throw new InvalidOperationException("RabbitMQ host was not found on configuration");
        }

        services.AddMassTransit(configurator =>
        {
            configurator.SetKebabCaseEndpointNameFormatter();
            configurator.UsingRabbitMq((context, rabbitmq) =>
            {
                rabbitmq.Host(host, virtualHost, hostConfigurator =>
                {
                    hostConfigurator.Username(username);
                    hostConfigurator.Password(password);
                });
                rabbitmq.ConfigureEndpoints(context);
            });
        });

        return services;
    }
}