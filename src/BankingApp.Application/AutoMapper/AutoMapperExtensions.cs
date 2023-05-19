using Microsoft.Extensions.DependencyInjection;
using System.Linq;
using System.Reflection;

namespace BankingApp.Application.AutoMapper;

public static class AutoMapperExtensions
{

    public static IServiceCollection AddAutoMapperFromAssemblies(this IServiceCollection serviceCollection
        , params string[] assemblies)
    {
        var loadedAssemblies = assemblies.Select(Assembly.Load).ToList();

        serviceCollection.AddAutoMapper(configAction => configAction.AllowNullCollections = true, loadedAssemblies);

        return serviceCollection;
    }

}