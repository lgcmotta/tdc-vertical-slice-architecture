namespace BankingApp.Infrastructure.Core.Hosting;

public class HostingEnvironmentVariables
{
    private const string DotNetEnvironment = "DOTNET_ENVIRONMENT";
    private const string AspNetCoreEnvironment = "ASPNETCORE_ENVIRONMENT";

    public static string? GetDotNetEnvironment()
        => Environment.GetEnvironmentVariable(DotNetEnvironment);

    public static string? GetAspNetCoreEnvironment()
        => Environment.GetEnvironmentVariable(AspNetCoreEnvironment);
}