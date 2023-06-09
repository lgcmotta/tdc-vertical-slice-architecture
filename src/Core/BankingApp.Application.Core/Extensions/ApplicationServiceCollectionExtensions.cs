using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using OpenTelemetry.Metrics;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;
using System.Reflection;

namespace BankingApp.Application.Core.Extensions;

public static class ApplicationServiceCollectionExtensions
{
    public static IServiceCollection AddValidators(this IServiceCollection services, params Assembly[] assemblies)
    {
        services.AddValidatorsFromAssemblies(assemblies);

        return services;
    }

    public static IServiceCollection AddOpenTelemetryConfiguration(
        this IServiceCollection services,
        string serviceName,
        string? serviceNamespace = null,
        string? serviceVersion = null)
    {
        services.AddOpenTelemetry()
            .ConfigureResource(resource => resource
                .AddService(
                    serviceName: serviceName,
                    serviceNamespace: serviceNamespace,
                    serviceVersion: serviceVersion,
                    autoGenerateServiceInstanceId: true
                )
                .AddTelemetrySdk()
                .AddEnvironmentVariableDetector()
            )
            .WithTracing(tracer => tracer
                .SetSampler<AlwaysOnSampler>()
                .AddAspNetCoreInstrumentation()
                .AddEntityFrameworkCoreInstrumentation(options => options.SetDbStatementForText = true)
                .AddSource("MassTransit")
                .AddOtlpExporter()
            )
            .WithMetrics(meter => meter
                .AddAspNetCoreInstrumentation()
                .AddRuntimeInstrumentation()
                .AddEventCountersInstrumentation(options => options
                    .AddEventSources(
                        "Microsoft.AspNetCore.Hosting",
                        "Microsoft.AspNetCore.Http.Connections",
                        "Microsoft-AspNetCore-Server-Kestrel",
                        "System.Net.Http",
                        "System.Net.NameResolution",
                        "System.Net.Security"
                    )
                )
                .AddOtlpExporter()
            );

        return services;
    }
}