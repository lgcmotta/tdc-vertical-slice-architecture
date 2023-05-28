using BankingApp.Accounts.API.Application;
using BankingApp.Accounts.API.Features.CreateAccount;
using BankingApp.Accounts.API.Infrastructure;
using BankingApp.Accounts.Domain;
using BankingApp.Application.Core.Behaviors;
using BankingApp.Infrastructure.Core.Extensions;
using MediatR.NotificationPublishers;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

builder.Services
    .AddEndpointsApiExplorer()
    .AddSwaggerGen()
    .AddMySqlDbContext<AccountHoldersDbContext>(
        builder.Configuration,
        builder.Configuration.GetConnectionString(nameof(AccountHoldersDbContext)),
        ServiceLifetime.Scoped,
        Assembly.Load("BankingApp.Infrastructure.Core")
    )
    .AddMediatR(configuration =>
    {
        configuration.RegisterServicesFromAssemblyContaining<Program>();
        configuration.AddOpenBehavior(typeof(ResilientTransactionBehavior<,>));
        configuration.NotificationPublisherType = typeof(TaskWhenAllPublisher);
    })
    .AddScoped<IAccountTokenGenerator, AccountTokenGenerator>()
    .AddUnitOfWork<AccountHoldersDbContext>()
    .AddRabbitMqMessaging(builder.Configuration);

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// app.UseHttpsRedirection();

app.MapPost("/api/accounts", CreateAccountEndpoint.PostAsync).WithOpenApi();

await app.Services.ApplyMigrationsAsync<AccountHoldersDbContext>();

await app.RunAsync();