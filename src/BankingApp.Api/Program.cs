using BankingApp.Api.Extensions;
using BankingApp.Application.AutoMapper;
using BankingApp.Application.DependencyInjection;
using BankingApp.Application.Hubs;
using BankingApp.Application.Policies;
using BankingApp.Application.Quartz;
using BankingApp.Application.Serilog;
using BankingApp.Infrastructure.EntityFramework;
using FluentValidation;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Quartz;
using Serilog;
using System;
using System.Linq;
using System.Reflection;

var assemblies = new[] { "BankingApp.Application", "BankingApp.Infrastructure", "BankingApp.Domain" }.Select(Assembly.Load).ToArray();

var builder = WebApplication.CreateBuilder();

builder.Services.AddSignalR();

builder.Services.AddHttpContextAccessor()
    .ConfigureSerilog(builder.Configuration)
    .AddAutoMapperFromAssemblies("BankingApp.Application")
    .AddCQRS(assemblies)
    .AddValidatorsFromAssemblies(assemblies)
    .AddBankingAppServices()
    .AddAccountsDbContext(builder.Configuration)
    .AddStackExchangeRedisCache(options =>
    {
        options.Configuration = builder.Configuration.GetConnectionString("Redis");
        options.InstanceName = nameof(BankingApp);
    })
    .AddQuartzJobs()
    .AddQuartzHostedService(quartz => quartz.WaitForJobsToComplete = true)
    .ConfigureCors()
    .ConfigureApiVersion()
    .AddRoutingWithLowerCaseUrls()
    .ConfigureSwaggerGen()
    .AddControllers();

builder.Services.ConfigureControllers();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseSwagger();
    app.UseSwaggerUI(options => options.SwaggerEndpoint("/swagger/v1/swagger.json", "BankingApp.Api v1"));
}

app.UseCors()
    .UseHttpsRedirection()
    .UseRouting()
    .UseEndpoints(endpoints =>
    {
        endpoints.MapControllers();
        endpoints.MapHub<AccountsHub>("/accounts/hub");
    });

var policy = PolicyFactory.CreateAsyncRetryPolicy(Log.Logger);

await policy.ExecuteAsync(async () => { await app.Services.MigrateDbContextAsync(); });

try
{
    await app.RunAsync();
}
catch (Exception exception)
{
    Log.Logger.Fatal(exception, "Host terminated unexpectedly");
}
finally
{
    Log.CloseAndFlush();
}