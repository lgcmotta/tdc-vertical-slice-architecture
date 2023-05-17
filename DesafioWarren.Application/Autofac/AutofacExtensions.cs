using System.Linq;
using System.Reflection;
using Autofac;

namespace DesafioWarren.Application.Autofac
{
    public static class AutofacExtensions
    {
        public static void AddAutofacModules(this ContainerBuilder containerBuilder)
        {
            containerBuilder.RegisterModule(new DesafioWarrenModule());

            var assemblies = new[]
            {
                "DesafioWarren.Application",
                "DesafioWarren.Infrastructure",
                "DesafioWarren.Domain"
            }
            .Select(Assembly.Load)
            .ToArray();

            containerBuilder.RegisterModule(new MediatorModule(assemblies));
            
            containerBuilder.RegisterModule(new ValidatorModule(assemblies));
        }
    }
}