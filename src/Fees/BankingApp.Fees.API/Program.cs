using BankingApp.Application.Core.Behaviors;
using BankingApp.Application.Core.Extensions;
using BankingApp.Application.Core.Middlewares;
using BankingApp.Fees.API.Infrastructure;
using BankingApp.Fees.API.Infrastructure.Handlers;
using BankingApp.Infrastructure.Core.Extensions;
using BankingApp.Infrastructure.Core.Handlers;
using MediatR.NotificationPublishers;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

builder.Services
    .AddEndpointsApiExplorer()
    .AddSwaggerGen()
    .AddMySqlDbContext<AccountFeesDbContext>(
        builder.Configuration,
        builder.Configuration.GetConnectionString(nameof(AccountFeesDbContext)),
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
    .AddUnitOfWork<AccountFeesDbContext>()
    .AddRabbitMqMessaging(builder.Configuration);

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseMiddleware<ExceptionHandlerMiddleware>();

app.Run();