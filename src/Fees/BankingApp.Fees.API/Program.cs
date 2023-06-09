using BankingApp.Application.Core.Behaviors;
using BankingApp.Application.Core.Extensions;
using BankingApp.Application.Core.Middlewares;
using BankingApp.Fees.API.Features.OverdraftFee;
using BankingApp.Fees.API.Features.ProfitFee;
using BankingApp.Fees.API.Infrastructure;
using BankingApp.Fees.API.Infrastructure.Handlers;
using BankingApp.Infrastructure.Core.Extensions;
using BankingApp.Infrastructure.Core.Handlers;
using MediatR.NotificationPublishers;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

var feesAssembly = typeof(Program).Assembly;
var feesAssemblyName = feesAssembly.GetName();

builder.Logging.ConfigureSerilogForOpenTelemetry();

builder.Services
    .AddMySqlDbContext<AccountFeesDbContext>(
        builder.Configuration,
        builder.Configuration.GetConnectionString(nameof(AccountFeesDbContext)),
        ServiceLifetime.Scoped,
        Assembly.Load("BankingApp.Infrastructure.Core")
    )
    .AddMediatR(configuration =>
    {
        configuration.RegisterServicesFromAssemblyContaining<Program>();
        configuration.AddOpenBehavior(typeof(LoggingBehavior<,>));
        configuration.AddOpenBehavior(typeof(ValidationBehavior<,>));
        configuration.AddOpenBehavior(typeof(ResilientTransactionBehavior<,>));
        configuration.NotificationPublisherType = typeof(TaskWhenAllPublisher);
    })
    .AddValidators(feesAssembly)
    .AddSingleton<IExceptionHandler, ExceptionHandler>()
    .AddUnitOfWork<AccountFeesDbContext>()
    .AddOverdraftFeeBackgroundService(builder.Configuration)
    .AddProfitFeeBackgroundService(builder.Configuration)
    .AddRabbitMqMessaging(builder.Configuration, feesAssembly)
    .AddOpenTelemetryConfiguration(
        serviceName: "fees",
        serviceNamespace: "banking-app",
        serviceVersion: feesAssemblyName.Version?.ToString() ?? null);

var app = builder.Build();

app.UseMiddleware<ExceptionHandlerMiddleware>();

await app.Services.ApplyMigrationsAsync<AccountFeesDbContext>();

app.Run();

[ExcludeFromCodeCoverage]
public partial class Program
{
    protected Program()
    { }
}