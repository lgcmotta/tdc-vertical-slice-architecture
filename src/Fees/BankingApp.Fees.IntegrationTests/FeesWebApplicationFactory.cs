// ReSharper disable ClassNeverInstantiated.Global

using BankingApp.Fees.IntegrationTests.Features.OverdraftFee;

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

            services.AddMassTransitTestHarness(configurator =>
            {
                configurator.AddConsumer<OverdraftFeeSettledDomainEventHandlerFixture.OverdraftConsumer>();

                configurator.SetKebabCaseEndpointNameFormatter();
                configurator.UsingRabbitMq((context, rabbitmq) =>
                {
                    rabbitmq.Host("localhost", "/", hostConfigurator =>
                    {
                        hostConfigurator.Username("guest");
                        hostConfigurator.Password("guest");
                    });

                    rabbitmq.ReceiveEndpoint("transactions-overdraft-fee", endpoint =>
                    {
                        endpoint.ConfigureConsumer<OverdraftFeeSettledDomainEventHandlerFixture.OverdraftConsumer>(context);
                    });

                    rabbitmq.ConfigureEndpoints(context);
                });
            });
        });
    }
}