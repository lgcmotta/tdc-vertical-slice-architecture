// ReSharper disable ClassNeverInstantiated.Global
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
}