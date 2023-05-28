using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace BankingApp.Application.Core.Extensions;

public static class ApplicationServiceCollectionExtensions
{
    public static IServiceCollection AddValidators(this IServiceCollection services, params Assembly[] assemblies)
    {
        services.AddValidatorsFromAssemblies(assemblies);

        return services;
    }
}