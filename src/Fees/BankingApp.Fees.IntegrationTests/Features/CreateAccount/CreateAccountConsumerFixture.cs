// ReSharper disable ClassNeverInstantiated.Global
namespace BankingApp.Fees.IntegrationTests.Features.CreateAccount;

public class CreateAccountConsumerFixture : FeesWebApplicationFactory
{
    private readonly Mock<ConsumeContext<AccountCreatedIntegrationEvent>> _consumeContextMock;
    private readonly Faker<AccountCreatedIntegrationEvent> _eventFaker;

    public CreateAccountConsumerFixture()
    {
        _consumeContextMock = new Mock<ConsumeContext<AccountCreatedIntegrationEvent>>();
        _eventFaker = new Faker<AccountCreatedIntegrationEvent>();
    }

    public ConsumeContext<AccountCreatedIntegrationEvent> ConsumeContext { get; private set; } = null!;

    public void SetupConsumeContext(AccountCreatedIntegrationEvent integrationEvent)
    {
        _consumeContextMock.Setup(_ => _.Message).Returns(() => integrationEvent);
        _consumeContextMock.Setup(_ => _.CancellationToken).Returns(() => CancellationToken.None);
        ConsumeContext = _consumeContextMock.Object;
    }

    public AccountCreatedIntegrationEvent CreatedIntegrationEvent()
    {
        return _eventFaker.CustomInstantiator(faker => new AccountCreatedIntegrationEvent(
                faker.Random.Guid(),
                faker.Name.FullName(),
                faker.Person.Cpf(),
                faker.Finance.Account(),
                faker.Finance.Currency().Code)
        )
        .Generate();
    }

    public class InvalidAccountCreatedIntegrationEvents : IEnumerable<object[]>
    {
        private readonly IEnumerable<object[]> _values = new[]
        {
            new object[] { new AccountCreatedIntegrationEvent(Guid.Empty, "John Doe", "999999999", "000000", "USD") },
            new object[] { new AccountCreatedIntegrationEvent(Guid.NewGuid(), "John Doe", "999999999", null!, "USD") },
            new object[] { new AccountCreatedIntegrationEvent(Guid.NewGuid(), "John Doe", "999999999", "", "USD") },
            new object[] { new AccountCreatedIntegrationEvent(Guid.NewGuid(), "John Doe", "999999999", " ", "USD") },
        };

        public IEnumerator<object[]> GetEnumerator() => _values.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}