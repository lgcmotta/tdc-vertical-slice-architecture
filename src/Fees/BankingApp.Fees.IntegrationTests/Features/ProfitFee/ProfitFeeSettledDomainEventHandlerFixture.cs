// ReSharper disable ClassNeverInstantiated.Global
using BankingApp.Infrastructure.Core.Consumers;

namespace BankingApp.Fees.IntegrationTests.Features.ProfitFee;

public class ProfitFeeSettledDomainEventHandlerFixture
{
    private readonly Faker<ProfitFeeSettledDomainEvent> _eventFaker;

    public ProfitFeeSettledDomainEventHandlerFixture()
    {
        _eventFaker = new Faker<ProfitFeeSettledDomainEvent>();
    }

    public ProfitFeeSettledDomainEvent CreateDomainEvent()
    {
        return _eventFaker
            .CustomInstantiator(faker => new ProfitFeeSettledDomainEvent(faker.Random.Guid(), faker.Finance.Amount()))
            .Generate();
    }

    public class ProfitFeeConsumer : IConsumer<AccountProfitFeeSettledIntegrationEvent>
    {
        public Task Consume(ConsumeContext<AccountProfitFeeSettledIntegrationEvent> context) => Task.CompletedTask;
    }

    public class ProfitFeeConsumerConfiguration : IConsumerConfiguration
    {
        public void ConfigureMassTransit(IBusRegistrationConfigurator configurator)
        {
            configurator.AddConsumer<ProfitFeeConsumer>();
        }

        public void ConfigureConsumers(IRabbitMqBusFactoryConfigurator rabbitmq, IBusRegistrationContext context)
        {
            rabbitmq.ReceiveEndpoint("transactions-profit-fee", configurator =>
            {
                configurator.ConfigureConsumer<ProfitFeeConsumer>(context);
            });
        }
    }

}