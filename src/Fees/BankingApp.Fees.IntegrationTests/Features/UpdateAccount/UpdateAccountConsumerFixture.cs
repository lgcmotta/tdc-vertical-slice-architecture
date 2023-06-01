namespace BankingApp.Fees.IntegrationTests.Features.UpdateAccount;

// ReSharper disable ClassNeverInstantiated.Global
public class UpdateAccountConsumerFixture : FeesWebApplicationFactory
{
    private readonly Mock<ConsumeContext<AccountUpdatedIntegrationEvent>> _consumeContextMock;
    private readonly Faker<AccountUpdatedIntegrationEvent> _eventFaker;
    private readonly Faker<Account> _accountFaker;

    public UpdateAccountConsumerFixture()
    {
        _consumeContextMock = new Mock<ConsumeContext<AccountUpdatedIntegrationEvent>>();
        _eventFaker = new Faker<AccountUpdatedIntegrationEvent>();
        _accountFaker = new Faker<Account>();
    }
    public ConsumeContext<AccountUpdatedIntegrationEvent> ConsumeContext { get; private set; } = null!;

    public void SetupConsumeContext(AccountUpdatedIntegrationEvent integrationEvent)
    {
        _consumeContextMock.Setup(_ => _.Message).Returns(() => integrationEvent);
        _consumeContextMock.Setup(_ => _.CancellationToken).Returns(() => CancellationToken.None);
        ConsumeContext = _consumeContextMock.Object;
    }

    public AccountUpdatedIntegrationEvent CreateIntegrationEvent(Guid holderId)
    {
        return _eventFaker.CustomInstantiator(faker => new AccountUpdatedIntegrationEvent(
                holderId,
                faker.Name.FullName(),
                faker.Finance.Account(),
                faker.Finance.Currency().Code
            ))
            .Generate();
    }

    public Account CreateAccount()
    {
        return _accountFaker
            .RuleFor(account => account.Id, faker => faker.Random.Guid())
            .RuleFor(account => account.Token, faker => faker.Finance.Account())
            .Generate();
    }

    public class InvalidAccountUpdatedIntegrationEvents : IEnumerable<object[]>
    {

        private readonly IEnumerable<object[]> _values = new[]
        {
            new object[] { new AccountUpdatedIntegrationEvent(Guid.Empty, "John Doe", "000000", "USD") },
            new object[] { new AccountUpdatedIntegrationEvent(Guid.NewGuid(), "John Doe", "", "USD") },
            new object[] { new AccountUpdatedIntegrationEvent(Guid.NewGuid(), "John Doe", " ", "USD") },
        };

        public IEnumerator<object[]> GetEnumerator() => _values.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }


}