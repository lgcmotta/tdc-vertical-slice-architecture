using Autofac;
using BankingApp.Application.Queries;
using BankingApp.Application.Services.Cache;
using BankingApp.Application.Services.Identity;
using BankingApp.Domain.Repositories;
using BankingApp.Infrastructure.EntityFramework.Repositories;

namespace BankingApp.Application.Autofac;

public class BankingAppModule : Module
{
    protected override void Load(ContainerBuilder builder)
    {
        builder.RegisterType<AccountsRepository>()
            .As<IAccountRepository>()
            .InstancePerLifetimeScope();
            
        builder.RegisterType<AccountsQueryWrapper>()
            .As<IAccountsQueryWrapper>()
            .InstancePerLifetimeScope();

        builder.RegisterType<IdentityService>()
            .As<IIdentityService>()
            .InstancePerLifetimeScope();
           
        builder.RegisterType<CachedConnectedAccountsManager>()
            .As<ICachedConnectedAccountsManager>()
            .SingleInstance();
    }
}