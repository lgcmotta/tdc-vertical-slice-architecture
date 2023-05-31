using BankingApp.Infrastructure.Core.Consumers;
using MassTransit;

namespace BankingApp.Fees.API.Features.UpdateBalance;

public class UpdateBalanceConsumerConfiguration : IConsumerConfiguration
{
    public void ConfigureMassTransit(IBusRegistrationConfigurator configurator)
    {
        configurator.AddConsumer<UpdateBalanceConsumer>();
    }

    public void ConfigureConsumers(IRabbitMqBusFactoryConfigurator rabbitmq, IBusRegistrationContext context)
    {
        rabbitmq.ReceiveEndpoint("fees-update-balance", configurator =>
        {
            configurator.ConfigureConsumer<UpdateBalanceConsumer>(context);
        });
    }
}