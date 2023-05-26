using Microsoft.Extensions.Configuration;

namespace BankingApp.Infrastructure.Core.Factories;

public static class ConfigurationFactory
{
    public static IConfiguration CreateConfiguration()
    {
        var configurationBuilder = new ConfigurationBuilder();

        configurationBuilder.AddJsonFile("appsettings.json");

        var aspNetCoreEnvironment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");

        var dotnetCoreEnvironment = Environment.GetEnvironmentVariable("DOTNET_ENVIRONMENT");

        if (!string.IsNullOrWhiteSpace(aspNetCoreEnvironment))
        {
            configurationBuilder.AddJsonFile(
                path: $"appsettings.{aspNetCoreEnvironment}.json",
                optional: true,
                reloadOnChange: true
            );
        }

        if (!string.IsNullOrWhiteSpace(dotnetCoreEnvironment))
        {
            configurationBuilder.AddJsonFile(
                path: $"appsettings.{dotnetCoreEnvironment}.json",
                optional: true,
                reloadOnChange: true
            );
        }

        configurationBuilder.AddEnvironmentVariables();

        return configurationBuilder.Build();
    }
}