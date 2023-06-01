namespace BankingApp.Fees.IntegrationTests.Features.ProfitFee;

[Collection("FeesWebApplicationFactory")]
public class ProfitFeeSettledDomainEventHandlerTests : IClassFixture<ProfitFeeSettledDomainEventHandlerFixture>
{
    private readonly ProfitFeeSettledDomainEventHandlerFixture _fixture;
    private readonly FeesWebApplicationFactory _factory;

    public ProfitFeeSettledDomainEventHandlerTests(ProfitFeeSettledDomainEventHandlerFixture fixture, FeesWebApplicationFactory factory)
    {
        _fixture = fixture;
        _factory = factory;
    }

    [Fact]
    public async Task ProfitFeeSettledDomainEventHandler_WhenEventIsSent_ShouldPublishToConsumers()
    {
        // Arrange
        var harness = _factory.Services.GetTestHarness();

        var domainEvent = _fixture.CreateDomainEvent();

        var handler = new ProfitFeeSettledDomainEventHandler(harness.Bus);

        var consumerHarness = harness.GetConsumerHarness<ProfitFeeSettledDomainEventHandlerFixture.ProfitFeeConsumer>();

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