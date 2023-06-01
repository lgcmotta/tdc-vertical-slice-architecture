namespace BankingApp.Fees.IntegrationTests.Features.CreateAccount;

[Collection("FeesWebApplicationFactory")]
public class CreateAccountCommandHandlerTests : IClassFixture<CreateAccountCommandHandlerFixture>
{
    private readonly CreateAccountCommandHandlerFixture _fixture;
    private readonly FeesWebApplicationFactory _factory;

    public CreateAccountCommandHandlerTests(CreateAccountCommandHandlerFixture fixture, FeesWebApplicationFactory factory)
    {
        _fixture = fixture;
        _factory = factory;
    }

    [Fact]
    public async Task CreateAccountCommandHandler_WhenAccountExists_ShouldThrowAccountHolderConflictException()
    {
        // Arrange
        using var scope = _factory.Services.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<AccountFeesDbContext>();

        var account = _fixture.CreateAccount();

        await context.AddAsync(account).ConfigureAwait(continueOnCapturedContext: false);
        await context.SaveChangesAsync().ConfigureAwait(continueOnCapturedContext: false);

        var command = _fixture.CreateCommand(account.Id);

        var handler = new CreateAccountCommandHandler(context);

        // Act & Assert
        await Assert.ThrowsAsync<AccountHolderConflictException>(() => handler.Handle(command, CancellationToken.None))
            .ConfigureAwait(continueOnCapturedContext: false);
    }

    [Fact]
    public async Task CreateAccountCommandHandler_WhenAccountDontExists_ShouldCreateAccount()
    {
        // Arrange
        using var scope = _factory.Services.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<AccountFeesDbContext>();

        var command = _fixture.CreateCommand();

        var handler = new CreateAccountCommandHandler(context);

        // Act
        await handler.Handle(command, CancellationToken.None).ConfigureAwait(continueOnCapturedContext: false);
        await context.SaveChangesAsync().ConfigureAwait(continueOnCapturedContext: false);

        var account = await context.Accounts.FirstOrDefaultAsync(account => account.Id == command.HolderId)
            .ConfigureAwait(continueOnCapturedContext: false);

        // Assert
        account.Should().NotBeNull();
    }
}