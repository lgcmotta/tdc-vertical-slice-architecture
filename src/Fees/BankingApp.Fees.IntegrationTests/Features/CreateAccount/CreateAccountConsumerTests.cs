namespace BankingApp.Fees.IntegrationTests.Features.CreateAccount;

public class CreateAccountConsumerTests : IClassFixture<CreateAccountConsumerFixture>
{
    private readonly CreateAccountConsumerFixture _fixture;

    public CreateAccountConsumerTests(CreateAccountConsumerFixture fixture)
    {
        _fixture = fixture;
    }

    [Theory]
    [ClassData(typeof(CreateAccountConsumerFixture.InvalidAccountCreatedIntegrationEvents))]
    public async Task CreateAccountConsumer_WhenIntegrationEventIsInvalid_ShouldThrowValidationFailedException(AccountCreatedIntegrationEvent integrationEvent)
    {
        // Arrange
        using var scope = _fixture.Services.CreateScope();
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
        using var scope = _fixture.Services.CreateScope();
        var mediator = scope.ServiceProvider.GetRequiredService<IMediator>();
        var context = scope.ServiceProvider.GetRequiredService<AccountFeesDbContext>();

        var integrationEvent = _fixture.CreatedIntegrationEvent();

        _fixture.SetupConsumeContext(integrationEvent);

        var consumer = new CreateAccountConsumer(mediator);

        // Act
        await consumer.Consume(_fixture.ConsumeContext);

        var account = await context.Accounts.FirstOrDefaultAsync(account => account.Id == integrationEvent.HolderId)
            .ConfigureAwait(continueOnCapturedContext: false);

        // Assert
        account.Should().NotBeNull();
    }
}