// ReSharper disable ClassNeverInstantiated.Global
using BankingApp.Infrastructure.Core.Consumers;

namespace BankingApp.Fees.IntegrationTests.Features.OverdraftFee;

public class OverdraftFeeSettledDomainEventHandlerFixture
{
    private readonly Faker<OverdraftFeeSettledDomainEvent> _eventFaker;

    public OverdraftFeeSettledDomainEventHandlerFixture()
    {
        _eventFaker = new Faker<OverdraftFeeSettledDomainEvent>();
    }

    public OverdraftFeeSettledDomainEvent GetDomainEvent()
    {
        return _eventFaker
            .CustomInstantiator(faker => new OverdraftFeeSettledDomainEvent(faker.Random.Guid(), faker.Finance.Amount()))
            .Generate();
    }

    public class OverdraftConsumer : IConsumer<AccountOverdraftSettledIntegrationEvent>
    {
        public Task Consume(ConsumeContext<AccountOverdraftSettledIntegrationEvent> context) => Task.CompletedTask;
    }

    public class OverdraftConsumerOptions : IConsumerConfiguration
    {
        public void ConfigureMassTransit(IBusRegistrationConfigurator configurator)
        {
            configurator.AddConsumer<OverdraftConsumer>();
        }

        public void ConfigureConsumers(IRabbitMqBusFactoryConfigurator rabbitmq, IBusRegistrationContext context)
        {
            rabbitmq.ReceiveEndpoint("transactions-overdraft-fee", configurator =>
            {
                configurator.ConfigureConsumer<OverdraftConsumer>(context);
            });
        }
    }
}