using BankingApp.Infrastructure.Core.Consumers;
using MassTransit;

namespace BankingApp.Transactions.API.Features.ApplyProfitFee;

public class ApplyProfitFeeConsumerConfiguration : IConsumerConfiguration
{
    public void ConfigureMassTransit(IBusRegistrationConfigurator configurator)
    {
        configurator.AddConsumer<ApplyProfitFeeConsumer>();
    }

    public void ConfigureConsumers(IRabbitMqBusFactoryConfigurator rabbitmq, IBusRegistrationContext context)
    {
        rabbitmq.ReceiveEndpoint("transactions-profit-fee", configurator =>
        {
            configurator.ConfigureConsumer<ApplyProfitFeeConsumer>(context);
        });
    }
}