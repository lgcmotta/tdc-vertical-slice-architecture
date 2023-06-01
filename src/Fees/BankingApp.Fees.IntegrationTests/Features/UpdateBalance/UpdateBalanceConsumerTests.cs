namespace BankingApp.Fees.IntegrationTests.Features.UpdateBalance;

[Collection("FeesWebApplicationFactory")]
public class UpdateBalanceConsumerTests : IClassFixture<UpdateBalanceConsumerFixture>
{
    private readonly UpdateBalanceConsumerFixture _fixture;
    private readonly FeesWebApplicationFactory _factory;

    private UpdateBalanceConsumerTests(UpdateBalanceConsumerFixture fixture)
    {
        _fixture = fixture;
        _factory = new FeesWebApplicationFactory();
    }

    public UpdateBalanceConsumerTests(UpdateBalanceConsumerFixture fixture, FeesWebApplicationFactory factory) : this(fixture)
    {
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
        var harness = _factory.Services.GetTestHarness();
        using var scope1 = _factory.Services.CreateScope();
        var context1 = scope1.ServiceProvider.GetRequiredService<AccountFeesDbContext>();

        var lastBalanceChange = new DateTime(2023, 05, 31);
        var account = _fixture.CreateAccount(lastBalanceChange: lastBalanceChange);

        await context1.Accounts.AddAsync(account).ConfigureAwait(continueOnCapturedContext: false);
        await context1.SaveChangesAsync().ConfigureAwait(continueOnCapturedContext: false);

        await context1.DisposeAsync();
        scope1.Dispose();

        var integrationEvent = _fixture.CreateIntegrationEvent(account.Id);


        var consumerHarness = harness.GetConsumerHarness<UpdateBalanceConsumer>();

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
        updatedAccount!.CurrentBalanceInUSD.Should().Be(new Money(integrationEvent.Balance));
        updatedAccount.LastBalanceChange.Should().BeAfter(lastBalanceChange);
    }
}