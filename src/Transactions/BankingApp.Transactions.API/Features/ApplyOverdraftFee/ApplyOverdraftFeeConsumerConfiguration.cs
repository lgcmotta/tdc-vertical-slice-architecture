using BankingApp.Infrastructure.Core.Consumers;
using MassTransit;

namespace BankingApp.Transactions.API.Features.ApplyOverdraftFee;

public class ApplyOverdraftFeeConsumerConfiguration : IConsumerConfiguration
{
    public void ConfigureMassTransit(IBusRegistrationConfigurator configurator)
    {
        configurator.AddConsumer<ApplyOverdraftFeeConsumer>();
    }

    public void ConfigureConsumers(IRabbitMqBusFactoryConfigurator rabbitmq, IBusRegistrationContext context)
    {
        rabbitmq.ReceiveEndpoint("transactions-overdraft-fee", configurator =>
        {
            configurator.ConfigureConsumer<ApplyOverdraftFeeConsumer>(context);
        });
    }
}