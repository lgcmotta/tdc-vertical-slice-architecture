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
        // builder.RegisterType<AccountsRepository>()
        //     .As<IAccountRepository>()
        //     .InstancePerLifetimeScope();
        //
        // builder.RegisterType<AccountsQueryWrapper>()
        //     .As<IAccountsQueryWrapper>()
        //     .InstancePerLifetimeScope();
        //
        // builder.RegisterType<IdentityService>()
        //     .As<IIdentityService>()
        //     .InstancePerLifetimeScope();
        //
        // builder.RegisterType<CachedConnectedAccountsManager>()
        //     .As<ICachedConnectedAccountsManager>()
        //     .SingleInstance();

        return services;
    }
}