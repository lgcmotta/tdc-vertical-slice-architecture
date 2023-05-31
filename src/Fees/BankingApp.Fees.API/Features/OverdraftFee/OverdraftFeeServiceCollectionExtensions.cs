namespace BankingApp.Fees.API.Features.OverdraftFee;

public static class OverdraftFeeServiceCollectionExtensions
{
    public static IServiceCollection AddOverdraftFeeService(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<OverdraftFeeOptions>(configuration.GetSection("BackgroundServices:OverdraftFee"));

        services.AddHostedService<OverdraftFeeBackgroundService>();

        return services;
    }
}