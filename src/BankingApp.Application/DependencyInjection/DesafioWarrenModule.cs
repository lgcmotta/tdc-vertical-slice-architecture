using BankingApp.Application.Queries;
using BankingApp.Application.Services.Cache;
using BankingApp.Application.Services.Identity;
using BankingApp.Domain.Repositories;
using BankingApp.Infrastructure.EntityFramework.Repositories;
using Microsoft.Extensions.DependencyInjection;

namespace BankingApp.Application.DependencyInjection;

public static class BankingAppServiceCollectionExtensions
{
    public static IServiceCollection AddBankingAppServices(this IServiceCollection services)
    {
        services.AddScoped<IAccountRepository, AccountsRepository>();
        services.AddScoped<IAccountsQueryWrapper, AccountsQueryWrapper>();
        services.AddScoped<IAccountsQueryWrapper, AccountsQueryWrapper>();
        services.AddScoped<IIdentityService, IdentityService>();
        services.AddScoped<ICachedConnectedAccountsManager, CachedConnectedAccountsManager>();

        return services;
    }
}