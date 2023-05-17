using Autofac;
using DesafioWarren.Application.Queries;
using DesafioWarren.Application.Services.Cache;
using DesafioWarren.Application.Services.Identity;
using DesafioWarren.Domain.Repositories;
using DesafioWarren.Infrastructure.EntityFramework.Repositories;

namespace DesafioWarren.Application.Autofac
{
    public class DesafioWarrenModule : Module
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
}