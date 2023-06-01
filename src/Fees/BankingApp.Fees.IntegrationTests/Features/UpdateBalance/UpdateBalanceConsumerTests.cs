namespace BankingApp.Fees.IntegrationTests.Features.UpdateBalance;

[Collection("FeesWebApplicationFactory")]
public class UpdateBalanceConsumerTests : IClassFixture<UpdateBalanceConsumerFixture>
{
    private readonly UpdateBalanceConsumerFixture _fixture;
    private readonly FeesWebApplicationFactory _factory;

    public UpdateBalanceConsumerTests(UpdateBalanceConsumerFixture fixture, FeesWebApplicationFactory factory)
    {
        _fixture = fixture;
        _factory = factory;
    }

    [Fact]
    public async Task UpdateBalanceConsumer_WhenHolderIsEmptyGuid_ShouldThrowValidationFailedException()
    {
        // Arrange
        using var scope = _factory.Services.CreateScope();
        var mediator = scope.ServiceProvider.GetRequiredService<IMediator>();

        var integrationEvent = _fixture.CreateIntegrationEvent(Guid.Empty);

        _fixture.SetupConsumeContext(integrationEvent);

        var consumer = new UpdateBalanceConsumer(mediator);

        // Act & Assert
        await Assert.ThrowsAsync<ValidationFailedException>(() => consumer.Consume(_fixture.ConsumeContext))
            .ConfigureAwait(continueOnCapturedContext: false);
    }

    [Fact]
    public async Task UpdateBalanceConsumer_WhenAccountDontExists_ShouldThrowAccountNotFoundException()
    {
        using var scope = _factory.Services.CreateScope();
        var mediator = scope.ServiceProvider.GetRequiredService<IMediator>();

        var integrationEvent = _fixture.CreateIntegrationEvent();

        _fixture.SetupConsumeContext(integrationEvent);

        var consumer = new UpdateBalanceConsumer(mediator);

        // Act & Assert
        await Assert.ThrowsAsync<AccountNotFoundException>(() => consumer.Consume(_fixture.ConsumeContext))
            .ConfigureAwait(continueOnCapturedContext: false);
    }

    [Fact]
    public async Task UpdateBalanceConsumer_WhenAccountExists_ShouldUpdateBalanceAndLastBalanceChange()
    {
        // Arrange
        using var scope = _factory.Services.CreateScope();
        var mediator = scope.ServiceProvider.GetRequiredService<IMediator>();
        var context = scope.ServiceProvider.GetRequiredService<AccountFeesDbContext>();

        var lastBalanceChange = new DateTime(2023, 05, 31);
        var account = _fixture.CreateAccount(lastBalanceChange: lastBalanceChange);
        await context.Accounts.AddAsync(account).ConfigureAwait(continueOnCapturedContext: false);
        await context.SaveChangesAsync().ConfigureAwait(continueOnCapturedContext: false);

        var integrationEvent = _fixture.CreateIntegrationEvent(account.Id);

        _fixture.SetupConsumeContext(integrationEvent);

        var consumer = new UpdateBalanceConsumer(mediator);

        // Act
        await consumer.Consume(_fixture.ConsumeContext);
        await context.SaveChangesAsync().ConfigureAwait(continueOnCapturedContext: false);

        // Assert
        account.CurrentBalanceInUSD.Should().Be(new Money(integrationEvent.Balance));
        account.LastBalanceChange.Should().BeAfter(lastBalanceChange);
    }
}