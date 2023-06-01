// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable PossibleMultipleEnumeration
namespace BankingApp.Fees.IntegrationTests;

[CollectionDefinition("FeesWebApplicationFactory")]
public class FeesWebApplicationFactoryCollection : ICollectionFixture<FeesWebApplicationFactory>
{ }

public class FeesWebApplicationFactory : WebApplicationFactory<Program>
{
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureServices(services =>
        {
            var profitFeeDescriptor = services.FirstOrDefault(descriptor =>
                descriptor.ImplementationType == typeof(ProfitFeeBackgroundService));
            var overdraftFeeDescriptor = services.FirstOrDefault(descriptor =>
                descriptor.ImplementationType == typeof(OverdraftFeeBackgroundService));

            if (profitFeeDescriptor is not null)
            {
                services.Remove(profitFeeDescriptor);
            }

            if (overdraftFeeDescriptor is not null)
            {
                services.Remove(overdraftFeeDescriptor);
            }

            var feesAssembly = Assembly.Load("BankingApp.Fees.API");
            var integrationTests = Assembly.Load("BankingApp.Fees.IntegrationTests");
            var consumerConfigurations = ConsumerConfigurationAssemblyScanner.Scan(feesAssembly, integrationTests);

            services.AddMassTransitTestHarness(configurator =>
            {
                foreach (var consumerConfiguration in consumerConfigurations)
                {
                    consumerConfiguration.ConfigureMassTransit(configurator);
                }

                configurator.SetKebabCaseEndpointNameFormatter();
                configurator.UsingRabbitMq((context, rabbitmq) =>
                {
                    rabbitmq.Host("localhost", "/", hostConfigurator =>
                    {
                        hostConfigurator.Username("guest");
                        hostConfigurator.Password("guest");
                    });

                    foreach (var consumerConfiguration in consumerConfigurations)
                    {
                        consumerConfiguration.ConfigureConsumers(rabbitmq, context);
                    }

                    rabbitmq.ConfigureEndpoints(context);
                });
            });
        });
    }
}