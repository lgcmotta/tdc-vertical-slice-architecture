using System.Linq;
using System.Reflection;
using Microsoft.Extensions.DependencyInjection;

namespace DesafioWarren.Application.AutoMapper
{
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
}