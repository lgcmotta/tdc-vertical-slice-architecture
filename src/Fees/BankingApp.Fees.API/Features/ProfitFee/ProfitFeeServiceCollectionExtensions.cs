namespace BankingApp.Fees.API.Features.ProfitFee;

public static class ProfitFeeServiceCollectionExtensions
{
    public static IServiceCollection AddProfitFeeBackgroundService(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<ProfitFeeOptions>(configuration.GetSection("BackgroundServices:ProfitFee"));

        services.AddHostedService<ProfitFeeBackgroundService>();

        return services;
    }
}