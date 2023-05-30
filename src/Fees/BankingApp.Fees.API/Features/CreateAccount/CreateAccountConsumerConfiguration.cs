using BankingApp.Infrastructure.Core.Consumers;
using MassTransit;

namespace BankingApp.Fees.API.Features.CreateAccount;

public class CreateAccountConsumerConfiguration : IConsumerConfiguration
{
    public void ConfigureMassTransit(IBusRegistrationConfigurator configurator)
    {
        configurator.AddConsumer<CreateAccountConsumer>();
    }

    public void ConfigureConsumers(IRabbitMqBusFactoryConfigurator rabbitmq, IBusRegistrationContext context)
    {
        rabbitmq.ReceiveEndpoint("fees-create-account", configurator =>
        {
            configurator.ConfigureConsumer<CreateAccountConsumer>(context);
        });
    }
}