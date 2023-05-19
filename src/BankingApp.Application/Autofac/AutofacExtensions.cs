using Autofac;
using System.Linq;
using System.Reflection;

namespace BankingApp.Application.Autofac;

public static class AutofacExtensions
{
    public static void AddAutofacModules(this ContainerBuilder containerBuilder)
    {
        containerBuilder.RegisterModule(new BankingAppModule());

        var assemblies = new[]
            {
                "BankingApp.Application",
                "BankingApp.Infrastructure",
                "BankingApp.Domain"
            }
            .Select(Assembly.Load)
            .ToArray();

        containerBuilder.RegisterModule(new MediatorModule(assemblies));

        containerBuilder.RegisterModule(new ValidatorModule(assemblies));
    }
}