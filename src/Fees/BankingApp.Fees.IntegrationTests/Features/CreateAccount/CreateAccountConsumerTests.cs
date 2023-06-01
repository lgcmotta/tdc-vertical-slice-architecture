namespace BankingApp.Fees.IntegrationTests.Features.CreateAccount;

[Collection("FeesWebApplicationFactory")]
public class CreateAccountConsumerTests : IClassFixture<CreateAccountConsumerFixture>
{
    private readonly CreateAccountConsumerFixture _fixture;
    private readonly FeesWebApplicationFactory _factory;

    private CreateAccountConsumerTests(CreateAccountConsumerFixture fixture)
    {
        _fixture = fixture;
        _factory = new FeesWebApplicationFactory();
    }

    public CreateAccountConsumerTests(CreateAccountConsumerFixture fixture, FeesWebApplicationFactory factory) : this(fixture)
    {
        _factory = factory;
    }

    [Theory]
    [ClassData(typeof(CreateAccountConsumerFixture.InvalidAccountCreatedIntegrationEvents))]
    public async Task CreateAccountConsumer_WhenIntegrationEventIsInvalid_ShouldThrowValidationFailedException(AccountCreatedIntegrationEvent integrationEvent)
    {
        // Arrange
        using var scope = _factory.Services.CreateScope();
        var mediator = scope.ServiceProvider.GetRequiredService<IMediator>();

        _fixture.SetupConsumeContext(integrationEvent);

        var consumer = new CreateAccountConsumer(mediator);

        // Act & Assert
        await Assert.ThrowsAsync<ValidationFailedException>(() => consumer.Consume(_fixture.ConsumeContext))
            .ConfigureAwait(continueOnCapturedContext: false);
    }

    [Fact]
    public async Task CreateAccountConsumer_WhenMessageIsReceived_ShouldCreateAccount()
    {
        // Arrange
        var harness = _factory.Services.GetTestHarness();
        using var scope = _factory.Services.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<AccountFeesDbContext>();

        var integrationEvent = _fixture.CreatedIntegrationEvent();

        var consumerHarness = harness.GetConsumerHarness<CreateAccountConsumer>();

        // Act
        await harness.Bus.Publish(integrationEvent, CancellationToken.None).ConfigureAwait(continueOnCapturedContext: false);

        await Task.Delay(5000).ConfigureAwait(continueOnCapturedContext: false);

        // Assert
        var account = await context.Accounts.FirstOrDefaultAsync(account => account.Id == integrationEvent.HolderId)
            .ConfigureAwait(continueOnCapturedContext: false);

        var consumed = await consumerHarness.Consumed.Any().ConfigureAwait(continueOnCapturedContext: false);

        account.Should().NotBeNull();
        consumed.Should().BeTrue();
    }
}