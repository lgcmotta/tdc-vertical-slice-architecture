using BankingApp.Infrastructure.Core.Hosting;
using Microsoft.Extensions.Configuration;

namespace BankingApp.Infrastructure.Core.Factories;

public static class ConfigurationFactory
{
    public static IConfiguration CreateConfiguration()
    {
        var configurationBuilder = new ConfigurationBuilder();

        configurationBuilder.AddJsonFile("appsettings.json");

        var aspNetCoreEnvironment = HostingEnvironmentVariables.GetAspNetCoreEnvironment();
        var dotnetCoreEnvironment = HostingEnvironmentVariables.GetDotNetEnvironment();

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