using BankingApp.Application.Core.Behaviors;
using BankingApp.Infrastructure.Core.Extensions;
using BankingApp.Transactions.API.Features.Deposits;
using BankingApp.Transactions.API.Features.PeriodStatement;
using BankingApp.Transactions.API.Features.Transfers;
using BankingApp.Transactions.API.Features.Withdraws;
using BankingApp.Transactions.API.Infrastructure;
using MediatR.NotificationPublishers;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

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
        configuration.AddOpenBehavior(typeof(ResilientTransactionBehavior<,>));
        configuration.NotificationPublisherType = typeof(TaskWhenAllPublisher);
    })
    .AddUnitOfWork<AccountsDbContext>()
    .AddRabbitMqMessaging(builder.Configuration, typeof(Program).Assembly);

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapPost("/transactions/deposit", DepositEndpoint.PostAsync).WithOpenApi();
app.MapPost("/transactions/withdraw", WithdrawEndpoint.PostAsync).WithOpenApi();
app.MapPost("/transactions/transfer", TransferEndpoint.PostAsync).WithOpenApi();
app.MapGet("/accounts/{token}/statements/period", PeriodStatementEndpoint.GetAsync).WithOpenApi();

await app.Services.ApplyMigrationsAsync<AccountsDbContext>();

await app.RunAsync();