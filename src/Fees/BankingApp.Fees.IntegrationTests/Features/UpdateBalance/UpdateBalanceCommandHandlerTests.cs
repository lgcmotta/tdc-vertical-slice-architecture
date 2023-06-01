namespace BankingApp.Fees.IntegrationTests.Features.UpdateBalance;

[Collection("FeesWebApplicationFactory")]
public class UpdateBalanceCommandHandlerTests : IClassFixture<UpdateBalanceCommandHandlerFixture>
{
    private readonly UpdateBalanceCommandHandlerFixture _fixture;
    private readonly FeesWebApplicationFactory _factory;

    private UpdateBalanceCommandHandlerTests(UpdateBalanceCommandHandlerFixture fixture)
    {
        _fixture = fixture;
        _factory = new FeesWebApplicationFactory();
    }

    public UpdateBalanceCommandHandlerTests(UpdateBalanceCommandHandlerFixture fixture, FeesWebApplicationFactory factory) : this(fixture)
    {
        _factory = factory;
    }

    [Fact]
    public async Task UpdateBalanceCommandHandler_WhenAccountNotExists_ShouldThrowAccountNotFoundException()
    {
        // Arrange
        using var scope = _factory.Services.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<AccountFeesDbContext>();

        var command = _fixture.CreateCommand();

        var handler = new UpdateBalanceCommandHandler(context);

        // Act & Assert
        await Assert.ThrowsAsync<AccountNotFoundException>(() => handler.Handle(command, CancellationToken.None))
            .ConfigureAwait(continueOnCapturedContext: false);
    }

    [Fact]
    public async Task UpdateBalanceCommandHandler_WhenAccountExists_ShouldUpdateBalanceAndLastBalanceChange()
    {
        // Arrange
        using var scope = _factory.Services.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<AccountFeesDbContext>();

        var lastBalanceChange = new DateTime(2023, 5, 31);
        var balance = new Money(20.0m);
        var account = _fixture.CreateAccount(balance, lastBalanceChange);

        await context.Accounts.AddAsync(account).ConfigureAwait(continueOnCapturedContext: false);
        await context.SaveChangesAsync().ConfigureAwait(continueOnCapturedContext: false);

        var command = _fixture.CreateCommand(account.Id);

        var handler = new UpdateBalanceCommandHandler(context);

        // Act
        await handler.Handle(command, CancellationToken.None).ConfigureAwait(continueOnCapturedContext: false);
        await context.SaveChangesAsync().ConfigureAwait(continueOnCapturedContext: false);

        // Assert
        account.CurrentBalanceInUSD.Should().Be(new Money(command.Balance));
        account.LastBalanceChange.Should().BeAfter(lastBalanceChange);
    }
}