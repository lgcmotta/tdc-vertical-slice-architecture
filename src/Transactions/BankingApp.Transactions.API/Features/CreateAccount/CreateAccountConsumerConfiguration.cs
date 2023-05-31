using BankingApp.Infrastructure.Core.Consumers;
using MassTransit;

namespace BankingApp.Transactions.API.Features.CreateAccount;

public class CreateAccountConsumerConfiguration : IConsumerConfiguration
{
    public void ConfigureMassTransit(IBusRegistrationConfigurator configurator)
    {
        configurator.AddConsumer<CreateAccountConsumer>();
    }

    public void ConfigureConsumers(IRabbitMqBusFactoryConfigurator rabbitmq, IBusRegistrationContext context)
    {
        rabbitmq.ReceiveEndpoint("transactions-create-account", configurator =>
        {
            configurator.ConfigureConsumer<CreateAccountConsumer>(context);
        });
    }
}