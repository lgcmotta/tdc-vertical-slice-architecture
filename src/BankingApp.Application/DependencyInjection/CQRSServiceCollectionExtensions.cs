using BankingApp.Application.Behaviours;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace BankingApp.Application.DependencyInjection;

public static class CQRSServiceCollectionExtensions
{
    public static IServiceCollection AddCQRS(this IServiceCollection services, params Assembly[] assemblies)
    {
        // builder.RegisterMediatR(_assemblies);
        //
        // builder.RegisterGeneric(typeof(LoggingBehaviour<,>)).As(typeof(IPipelineBehavior<,>));
        //
        // builder.RegisterGeneric(typeof(ValidationBehaviour<,>)).As(typeof(IPipelineBehavior<,>));
        //
        // builder.RegisterGeneric(typeof(TransactionalBehaviour<,>)).As(typeof(IPipelineBehavior<,>));
        return services;
    }
}