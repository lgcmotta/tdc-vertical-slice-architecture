// ReSharper disable ClassNeverInstantiated.Global
namespace BankingApp.Fees.IntegrationTests.Features.UpdateBalance;

public class UpdateBalanceConsumerFixture
{
    private readonly Mock<ConsumeContext<AccountBalanceChangedIntegrationEvent>> _consumeContextMock;
    private readonly Faker<AccountBalanceChangedIntegrationEvent> _eventFaker;
    private readonly Faker<Account> _accountFaker;

    public UpdateBalanceConsumerFixture()
    {
        _consumeContextMock = new Mock<ConsumeContext<AccountBalanceChangedIntegrationEvent>>();
        _eventFaker = new Faker<AccountBalanceChangedIntegrationEvent>();
        _accountFaker = new Faker<Account>();
        Bogus.DataSets.Date.SystemClock = () => new DateTime(2023, 5, 1, 0, 0, 0);
    }

    public ConsumeContext<AccountBalanceChangedIntegrationEvent> ConsumeContext { get; private set; } = null!;

    public void SetupConsumeContext(AccountBalanceChangedIntegrationEvent integrationEvent)
    {
        _consumeContextMock.Setup(_ => _.Message).Returns(() => integrationEvent);
        _consumeContextMock.Setup(_ => _.CancellationToken).Returns(() => CancellationToken.None);
        ConsumeContext = _consumeContextMock.Object;
    }

    public AccountBalanceChangedIntegrationEvent CreateIntegrationEvent(Guid? holderId = null)
    {
        return _eventFaker.CustomInstantiator(faker => new AccountBalanceChangedIntegrationEvent(
                holderId ?? faker.Random.Guid(),
                faker.Finance.Amount()
            ))
            .Generate();
    }

    public Account CreateAccount(decimal? balance = null, DateTime? lastBalanceChange = null)
    {
        return _accountFaker
            .RuleFor(account => account.Id, faker => faker.Random.Guid())
            .RuleFor(account => account.Token, faker => faker.Finance.Account())
            .RuleFor(account => account.CurrentBalanceInUSD, faker => new Money(balance ?? faker.Finance.Amount()))
            .RuleFor(account => account.LastBalanceChange, faker => lastBalanceChange ?? faker.Date.Past())
            .Generate();
    }
}