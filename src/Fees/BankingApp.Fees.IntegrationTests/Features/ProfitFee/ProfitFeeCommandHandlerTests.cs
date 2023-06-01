namespace BankingApp.Fees.IntegrationTests.Features.ProfitFee;

[Collection("FeesWebApplicationFactory")]
public class ProfitFeeCommandHandlerTests : IClassFixture<ProfitFeeCommandHandlerFixture>
{
    private readonly ProfitFeeCommandHandlerFixture _fixture;
    private readonly FeesWebApplicationFactory _factory;

    public ProfitFeeCommandHandlerTests(ProfitFeeCommandHandlerFixture fixture, FeesWebApplicationFactory factory)
    {
        _fixture = fixture;
        _factory = factory;
    }

    [Fact]
    public async Task ProfitFeeCommandHandler_WhenRateIsLessThanOrEqualToDecimalZero_ShouldThrowValidationFailedException()
    {
        // Arrange
        using var scope = _factory.Services.CreateScope();
        var mediator = scope.ServiceProvider.GetRequiredService<IMediator>();

        const int balanceIdleInMinutes = -1;
        var command = _fixture.CreateCommand(decimal.Zero, balanceIdleInMinutes);

        // Act & Assert
        await Assert.ThrowsAsync<ValidationFailedException>(() => mediator.Send(command, CancellationToken.None))
            .ConfigureAwait(continueOnCapturedContext: false);
    }

    [Fact]
    public async Task ProfitFeeCommandHandler_WhenBalanceIsIdle_ShouldApplyProfitFee()
    {
        // Arrange
        using var scope = _factory.Services.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<AccountFeesDbContext>();

        var lastBalanceChange = new DateTime(2023, 5, 31);
        const decimal balance = 200.00m;
        const int balanceIdleInMinutes = -1;
        const decimal rate = 0.10m;

        var account = _fixture.CreateAccount(balance, lastBalanceChange);

        await context.Accounts.AddAsync(account).ConfigureAwait(continueOnCapturedContext: false);
        await context.SaveChangesAsync().ConfigureAwait(continueOnCapturedContext: false);

        var command = _fixture.CreateCommand(rate, balanceIdleInMinutes);

        var handler = new ProfitFeeCommandHandler(context);

        // Act
        await handler.Handle(command, CancellationToken.None).ConfigureAwait(continueOnCapturedContext: false);
        await context.SaveChangesAsync().ConfigureAwait(continueOnCapturedContext: false);

        // Assert
        account.CurrentBalanceInUSD.Should().Be(new Money(balance * rate + balance));
        account.FeeHistory.Should().NotBeEmpty().And.HaveCount(1);
        account.DomainEvents.Should().NotBeEmpty().And.HaveCount(1);
    }
}