using MassTransit;

namespace BankingApp.Infrastructure.Core.Consumers;

public interface IConsumerConfiguration
{
    void ConfigureMassTransit(IBusRegistrationConfigurator configurator);

    void ConfigureConsumers(IRabbitMqBusFactoryConfigurator rabbitmq, IBusRegistrationContext context);
}