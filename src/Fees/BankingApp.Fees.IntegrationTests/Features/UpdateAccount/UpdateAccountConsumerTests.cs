namespace BankingApp.Fees.IntegrationTests.Features.UpdateAccount;

public class UpdateAccountConsumerTests : IClassFixture<UpdateAccountConsumerFixture>
{
    private readonly UpdateAccountConsumerFixture _fixture;

    public UpdateAccountConsumerTests(UpdateAccountConsumerFixture fixture)
    {
        _fixture = fixture;
    }

    [Theory]
    [ClassData(typeof(UpdateAccountConsumerFixture.InvalidAccountUpdatedIntegrationEvents))]
    public async Task UpdateAccountConsumer_WhenIntegrationEventIsInvalid_ShouldThrowValidationFailedException(AccountUpdatedIntegrationEvent integrationEvent)
    {
        // Arrange
        using var scope = _fixture.Services.CreateScope();
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
        using var scope = _fixture.Services.CreateScope();
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
        using var scope = _fixture.Services.CreateScope();
        var mediator = scope.ServiceProvider.GetRequiredService<IMediator>();
        var context = scope.ServiceProvider.GetRequiredService<AccountFeesDbContext>();

        var account = _fixture.CreateAccount();

        await context.AddAsync(account).ConfigureAwait(continueOnCapturedContext: false);
        await context.SaveChangesAsync().ConfigureAwait(continueOnCapturedContext: false);

        var integrationEvent = _fixture.CreateIntegrationEvent(account.Id);

        _fixture.SetupConsumeContext(integrationEvent);

        var consumer = new UpdateAccountConsumer(mediator);

        // Act
        await consumer.Consume(_fixture.ConsumeContext);

        var updatedAccount = await context.Accounts.FirstOrDefaultAsync(updated => updated.Id == account.Id);

        // Assert
        updatedAccount.Should().NotBeNull();
        updatedAccount!.Token.Should().Be(integrationEvent.Token);
    }
}