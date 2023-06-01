namespace BankingApp.Fees.IntegrationTests.Features.OverdraftFee;

[Collection("FeesWebApplicationFactory")]
public class OverdraftFeeSettledDomainEventHandlerTests : IClassFixture<OverdraftFeeSettledDomainEventHandlerFixture>
{
    private readonly OverdraftFeeSettledDomainEventHandlerFixture _fixture;
    private readonly FeesWebApplicationFactory _factory;

    public OverdraftFeeSettledDomainEventHandlerTests(OverdraftFeeSettledDomainEventHandlerFixture fixture, FeesWebApplicationFactory factory)
    {
        _fixture = fixture;
        _factory = factory;
    }

    [Fact]
    public async Task OverdraftFeeSettledDomainEventHandler_WhenEventIsSent_ShouldPublishToConsumers()
    {
        // Arrange
        var harness = _factory.Services.GetTestHarness();

        var domainEvent = _fixture.CreateDomainEvent();

        var handler = new OverdraftFeeSettledDomainEventHandler(harness.Bus);

        var consumerHarness = harness.GetConsumerHarness<OverdraftFeeSettledDomainEventHandlerFixture.OverdraftFeeConsumer>();

        // Act
        await handler.Handle(domainEvent, CancellationToken.None).ConfigureAwait(continueOnCapturedContext: false);

        await Task.Delay(5000).ConfigureAwait(continueOnCapturedContext: false);

        var consumed = await consumerHarness.Consumed.Any().ConfigureAwait(continueOnCapturedContext: false);
        var published = await harness.Published.Any().ConfigureAwait(continueOnCapturedContext: false);

        // Assert
        consumed.Should().BeTrue();
        published.Should().BeTrue();
    }
}