namespace BankingApp.Fees.IntegrationTests.Features.UpdateAccount;

[Collection("FeesWebApplicationFactory")]
public class UpdateAccountConsumerTests : IClassFixture<UpdateAccountConsumerFixture>
{
    private readonly UpdateAccountConsumerFixture _fixture;
    private readonly FeesWebApplicationFactory _factory;

    private UpdateAccountConsumerTests(UpdateAccountConsumerFixture fixture)
    {
        _fixture = fixture;
        _factory = new FeesWebApplicationFactory();
    }

    public UpdateAccountConsumerTests(UpdateAccountConsumerFixture fixture, FeesWebApplicationFactory factory) : this(fixture)
    {
        _factory = factory;
    }

    [Theory]
    [ClassData(typeof(UpdateAccountConsumerFixture.InvalidAccountUpdatedIntegrationEvents))]
    public async Task UpdateAccountConsumer_WhenIntegrationEventIsInvalid_ShouldThrowValidationFailedException(AccountUpdatedIntegrationEvent integrationEvent)
    {
        // Arrange
        using var scope = _factory.Services.CreateScope();
        var mediator = scope.ServiceProvider.GetRequiredService<IMediator>();

        _fixture.SetupConsumeContext(integrationEvent);

        var consumer = new UpdateAccountConsumer(mediator);

        // Act & Assert
        await Assert.ThrowsAsync<ValidationFailedException>(() => consumer.Consume(_fixture.ConsumeContext))
            .ConfigureAwait(continueOnCapturedContext: false);
    }

    [Fact]
    public async Task UpdateAccountConsumer_WhenAccountDontExists_ShouldThrowAccountNotFoundException()
    {
        // Arrange
        using var scope = _factory.Services.CreateScope();
        var mediator = scope.ServiceProvider.GetRequiredService<IMediator>();

        var integrationEvent = _fixture.CreateIntegrationEvent(Guid.NewGuid());

        _fixture.SetupConsumeContext(integrationEvent);

        var consumer = new UpdateAccountConsumer(mediator);

        // Act & Assert
        await Assert.ThrowsAsync<AccountNotFoundException>(() => consumer.Consume(_fixture.ConsumeContext))
            .ConfigureAwait(continueOnCapturedContext: false);
    }

    [Fact]
    public async Task UpdateAccountConsumer_WhenMessageIsReceived_ShouldUpdateAccount()
    {
        // Arrange
        var harness = _factory.Services.GetTestHarness();
        using var scope1 = _factory.Services.CreateScope();
        var context1 = scope1.ServiceProvider.GetRequiredService<AccountFeesDbContext>();

        var account = _fixture.CreateAccount();

        await context1.Accounts.AddAsync(account).ConfigureAwait(continueOnCapturedContext: false);
        await context1.SaveChangesAsync().ConfigureAwait(continueOnCapturedContext: false);

        await context1.DisposeAsync();
        scope1.Dispose();

        var integrationEvent = _fixture.CreateIntegrationEvent(account.Id);

        var consumerHarness = harness.GetConsumerHarness<UpdateAccountConsumer>();

        // Act
        await harness.Bus.Publish(integrationEvent, CancellationToken.None).ConfigureAwait(continueOnCapturedContext: false);

        await Task.Delay(5000).ConfigureAwait(continueOnCapturedContext: false);

        // Assert
        var consumed = await consumerHarness.Consumed.Any().ConfigureAwait(continueOnCapturedContext: false);

        using var scope2 = _factory.Services.CreateScope();
        var context2 = scope2.ServiceProvider.GetRequiredService<AccountFeesDbContext>();

        var updatedAccount = await context2.Accounts
            .FirstOrDefaultAsync(updated => updated.Id == account.Id)
            .ConfigureAwait(continueOnCapturedContext: false);

        consumed.Should().BeTrue();
        updatedAccount!.Token.Should().Be(integrationEvent.Token);
    }
}