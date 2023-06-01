namespace BankingApp.Fees.IntegrationTests.Features.OverdraftFee;

[Collection("FeesWebApplicationFactory")]
public class OverdraftFeeCommandHandlerTests : IClassFixture<OverdraftFeeCommandHandlerFixture>
{
    private readonly OverdraftFeeCommandHandlerFixture _fixture;
    private readonly FeesWebApplicationFactory _factory;

    public OverdraftFeeCommandHandlerTests(OverdraftFeeCommandHandlerFixture fixture, FeesWebApplicationFactory factory)
    {
        _fixture = fixture;
        _factory = factory;
    }

    [Fact]
    public async Task OverdraftFeeCommandHandler_WhenRateIsLessThanOrEqualToDecimalZero_ShouldThrowValidationFailedException()
    {
        // Arrange
        using var scope = _factory.Services.CreateScope();
        var mediator = scope.ServiceProvider.GetRequiredService<IMediator>();

        var command = _fixture.CreateCommand(decimal.Zero);

        // Act & Assert
        await Assert.ThrowsAsync<ValidationFailedException>(() => mediator.Send(command, CancellationToken.None))
            .ConfigureAwait(continueOnCapturedContext: false);
    }

    [Fact]
    public async Task OverdraftFeeCommandHandler_WhenAccountsAreInOverdraft_ShouldApplyOverdraftFee()
    {
        // Arrange
        using var scope = _factory.Services.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<AccountFeesDbContext>();

        var lastBalanceChange = new DateTime(2023, 5, 31);
        const decimal balance = -200.00m;
        const decimal rate = 0.01m;

        var account = _fixture.CreateAccount(balance, lastBalanceChange);

        await context.Accounts.AddAsync(account).ConfigureAwait(continueOnCapturedContext: false);
        await context.SaveChangesAsync().ConfigureAwait(continueOnCapturedContext: false);

        var command = _fixture.CreateCommand(rate);

        var handler = new OverdraftFeeCommandHandler(context);
        // Act
        await handler.Handle(command, CancellationToken.None).ConfigureAwait(continueOnCapturedContext: false);
        await context.SaveChangesAsync().ConfigureAwait(continueOnCapturedContext: false);

        // Assert
        account.CurrentBalanceInUSD.Should().Be(new Money(balance * rate + balance));
        account.FeeHistory.Should().NotBeEmpty().And.HaveCount(1);
        account.DomainEvents.Should().NotBeEmpty().And.HaveCount(1);
    }
}