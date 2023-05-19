using Autofac.Extensions.DependencyInjection;
using BankingApp.Api;
using BankingApp.Application.Policies;
using BankingApp.Infrastructure.EntityFramework;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Serilog;
using System;

// TODO: migrate this app to use minimal APIs
// var builder = WebApplication.CreateBuilder();
//
// var app = builder.Build();
//
// await app.RunAsync();

try
{
    var host = CreateHostBuilder(args).Build();

    var policy = PolicyFactory.CreateAsyncRetryPolicy(Log.Logger);

    await policy.ExecuteAsync(async () => { await host.Services.MigrateDbContextAsync(); });

    await host.RunAsync();
}
catch (Exception exception)
{
    Log.Logger.Fatal(exception, "Host terminated unexpectedly");
}
finally
{
    Log.CloseAndFlush();
}

static IHostBuilder CreateHostBuilder(string[] args) =>
    Host.CreateDefaultBuilder(args)
        .UseServiceProviderFactory(new AutofacServiceProviderFactory())
        .UseSerilog()
        .ConfigureWebHostDefaults(webBuilder =>
        {
            webBuilder.UseStartup<Startup>();
        });