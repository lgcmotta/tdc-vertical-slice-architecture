using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using System;
using System.Threading.Tasks;
using Autofac.Extensions.DependencyInjection;
using DesafioWarren.Application.Policies;
using DesafioWarren.Infrastructure.EntityFramework;
using Serilog;

namespace DesafioWarren.Api
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            try
            {
                var host = CreateHostBuilder(args).Build();

                var policy = PolicyFactory.CreateAsyncRetryPolicy(Log.Logger);

                await policy.ExecuteAsync(async () => { await host.Services.MigrateDbContextAsync(); });

                await host.RunAsync();
            }
            catch (Exception exception)
            {
                Log.Logger.Fatal(exception, "Host terminated unexpectedly.");
            }
            finally
            {
                Log.CloseAndFlush();
            }
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .UseServiceProviderFactory(new AutofacServiceProviderFactory())
                .UseSerilog()
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
