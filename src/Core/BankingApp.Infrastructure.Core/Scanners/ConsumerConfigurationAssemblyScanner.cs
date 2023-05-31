using BankingApp.Infrastructure.Core.Consumers;
using System.Reflection;

namespace BankingApp.Infrastructure.Core.Scanners;

public static class ConsumerConfigurationAssemblyScanner
{
    public static IEnumerable<IConsumerConfiguration> Scan(params Assembly[] configurationAssemblies)
    {
        if (!configurationAssemblies.Any())
        {
            return Enumerable.Empty<IConsumerConfiguration>();
        }

        return configurationAssemblies
            .Distinct()
            .SelectMany(assembly => assembly.GetTypes())
            .Where(type => type is { IsClass: true, IsAbstract: false } &&
                           type.IsAssignableTo(typeof(IConsumerConfiguration)) &&
                           type.GetConstructor(
                               bindingAttr: BindingFlags.Instance | BindingFlags.Public,
                               binder: null,
                               types: Type.EmptyTypes,
                               modifiers: null) is not null)
            .Select(Activator.CreateInstance)
            .Cast<IConsumerConfiguration>();
    }
}