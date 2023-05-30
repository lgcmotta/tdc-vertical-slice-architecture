using BankingApp.Infrastructure.Core.Consumers;
using MassTransit;

namespace BankingApp.Fees.API.Features.UpdateAccount;

public class UpdateAccountConsumerConfiguration : IConsumerConfiguration
{
    public void ConfigureMassTransit(IBusRegistrationConfigurator configurator)
    {
        configurator.AddConsumer<UpdateAccountConsumer>();
    }

    public void ConfigureConsumers(IRabbitMqBusFactoryConfigurator rabbitmq, IBusRegistrationContext context)
    {
        rabbitmq.ReceiveEndpoint("fees-update-account", configurator =>
        {
            configurator.ConfigureConsumer<UpdateAccountConsumer>(context);
        });
    }
}