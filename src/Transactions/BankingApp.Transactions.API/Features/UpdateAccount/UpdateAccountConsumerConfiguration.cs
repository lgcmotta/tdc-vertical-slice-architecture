using BankingApp.Infrastructure.Core.Consumers;
using MassTransit;

namespace BankingApp.Transactions.API.Features.UpdateAccount;

public class UpdateAccountConsumerConfiguration : IConsumerConfiguration
{
    public void ConfigureMassTransit(IBusRegistrationConfigurator configurator)
    {
        configurator.AddConsumer<UpdateAccountConsumer>();
    }

    public void ConfigureConsumers(IRabbitMqBusFactoryConfigurator rabbitmq, IBusRegistrationContext context)
    {
        rabbitmq.ReceiveEndpoint("transactions-update-account", configurator =>
        {
            configurator.ConfigureConsumer<UpdateAccountConsumer>(context);
        });
    }
}