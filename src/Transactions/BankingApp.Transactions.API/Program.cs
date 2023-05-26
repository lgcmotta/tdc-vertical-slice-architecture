using BankingApp.Infrastructure.Core.Extensions;
using BankingApp.Transactions.API.Infrastructure;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddMySqlDbContext<AccountsDbContext>(
    builder.Configuration,
    builder.Configuration.GetConnectionString(nameof(AccountsDbContext)),
    ServiceLifetime.Scoped,
    Assembly.Load("BankingApp.Infrastructure.Core")
);

builder.Services.AddMediatR(configuration =>
{
    configuration.RegisterServicesFromAssemblyContaining<Program>();
});

builder.Services.AddRabbitMqMessaging(builder.Configuration);

var app = builder.Build();

app.MapGet("/", () => "Hello World!");

app.Run();