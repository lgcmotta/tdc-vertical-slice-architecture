using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Serilog;

namespace DesafioWarren.Application.Serilog
{
    public static class SerilogExtensions
    {
        public static IServiceCollection ConfigureSerilog(this IServiceCollection serviceCollection, IConfiguration configuration)
        {
            Log.Logger = new LoggerConfiguration()
                .WriteTo
                .Console()
                .Enrich
                .FromLogContext()
                .MinimumLevel
                .Information()
                .CreateLogger();

            serviceCollection.AddSingleton(Log.Logger);

            return serviceCollection;
        }
    }
}