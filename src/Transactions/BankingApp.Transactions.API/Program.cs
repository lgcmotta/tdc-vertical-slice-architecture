using BankingApp.Application.Core.Behaviors;
using BankingApp.Application.Core.Extensions;
using BankingApp.Application.Core.Middlewares;
using BankingApp.Infrastructure.Core.Extensions;
using BankingApp.Infrastructure.Core.Handlers;
using BankingApp.Transactions.API.Features.Deposits;
using BankingApp.Transactions.API.Features.PeriodStatement;
using BankingApp.Transactions.API.Features.Transfers;
using BankingApp.Transactions.API.Features.Withdraws;
using BankingApp.Transactions.API.Infrastructure;
using BankingApp.Transactions.API.Infrastructure.Handlers;
using MediatR.NotificationPublishers;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

var transactionsAssembly = typeof(Program).Assembly;

builder.Services
    .AddEndpointsApiExplorer()
    .AddSwaggerGen()
    .AddMySqlDbContext<AccountsDbContext>(
        builder.Configuration,
        builder.Configuration.GetConnectionString(nameof(AccountsDbContext)),
        ServiceLifetime.Scoped,
        Assembly.Load("BankingApp.Infrastructure.Core")
    )
    .AddMediatR(configuration =>
    {
        configuration.RegisterServicesFromAssemblyContaining<Program>();
        configuration.AddOpenBehavior(typeof(ValidationBehavior<,>));
        configuration.AddOpenBehavior(typeof(ResilientTransactionBehavior<,>));
        configuration.NotificationPublisherType = typeof(TaskWhenAllPublisher);
    })
    .AddValidators(transactionsAssembly)
    .AddSingleton<IExceptionHandler, ExceptionHandler>()
    .AddUnitOfWork<AccountsDbContext>()
    .AddRabbitMqMessaging(builder.Configuration, transactionsAssembly);

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseMiddleware<ExceptionHandlerMiddleware>();

app.MapPost("/api/transactions/{token}/deposit", DepositEndpoint.PostAsync).WithOpenApi();
app.MapPost("/api/transactions/{token}/withdraw", WithdrawEndpoint.PostAsync).WithOpenApi();
app.MapPost("/api/transactions/{token}/transfer", TransferEndpoint.PostAsync).WithOpenApi();
app.MapGet("/api/transactions/{token}/statements", PeriodStatementEndpoint.GetAsync).WithOpenApi();

await app.Services.ApplyMigrationsAsync<AccountsDbContext>();

await app.RunAsync();