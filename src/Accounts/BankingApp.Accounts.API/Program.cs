using BankingApp.Accounts.API.Application;
using BankingApp.Accounts.API.Features.ChangeToken;
using BankingApp.Accounts.API.Features.CreateAccount;
using BankingApp.Accounts.API.Features.RetrieveAccountDetails;
using BankingApp.Accounts.API.Features.RetrieveTokenHistory;
using BankingApp.Accounts.API.Features.UpdateAccount;
using BankingApp.Accounts.API.Features.UpdateAccountPartially;
using BankingApp.Accounts.API.Infrastructure;
using BankingApp.Accounts.API.Infrastructure.Handlers;
using BankingApp.Accounts.Domain;
using BankingApp.Application.Core.Behaviors;
using BankingApp.Application.Core.Extensions;
using BankingApp.Application.Core.Middlewares;
using BankingApp.Infrastructure.Core.Extensions;
using BankingApp.Infrastructure.Core.Handlers;
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
        configuration.AddOpenBehavior(typeof(ValidationBehavior<,>));
        configuration.AddOpenBehavior(typeof(ResilientTransactionBehavior<,>));
        configuration.NotificationPublisherType = typeof(TaskWhenAllPublisher);
    })
    .AddValidators(typeof(Program).Assembly)
    .AddSingleton<IExceptionHandler, ExceptionHandler>()
    .AddScoped<IAccountTokenGenerator, AccountTokenGenerator>()
    .AddUnitOfWork<AccountHoldersDbContext>()
    .AddRabbitMqMessaging(builder.Configuration);

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseMiddleware<ExceptionHandlerMiddleware>();

app.MapPost("/api/accounts", CreateAccountEndpoint.PostAsync).WithOpenApi();
app.MapPut("/api/accounts/{token}/", UpdateAccountEndpoint.PutAsync).WithOpenApi();
app.MapPatch("/api/accounts/{token}/", UpdateAccountPartiallyEndpoint.PatchAsync).WithOpenApi();
app.MapPost("/api/accounts/{token}/tokens", ChangeTokenEndpoint.PostAsync).WithOpenApi();
app.MapGet("/api/accounts/{token}/tokens", RetrieveTokenHistoryQueryEndpoint.GetAsync).WithOpenApi();
app.MapGet("/api/accounts/{token}", RetrieveAccountDetailsEndpoint.GetAsync).WithOpenApi();

await app.Services.ApplyMigrationsAsync<AccountHoldersDbContext>();

await app.RunAsync();