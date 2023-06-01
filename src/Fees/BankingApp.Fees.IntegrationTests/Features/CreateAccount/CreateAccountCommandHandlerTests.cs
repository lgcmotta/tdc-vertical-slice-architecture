using BankingApp.Fees.Domain.Exceptions;

namespace BankingApp.Fees.IntegrationTests.Features.CreateAccount;

public class CreateAccountCommandHandlerTests : IClassFixture<CreateAccountCommandHandlerFixture>
{
    private readonly CreateAccountCommandHandlerFixture _fixture;

    public CreateAccountCommandHandlerTests(CreateAccountCommandHandlerFixture fixture)
    {
        _fixture = fixture;
    }

    [Fact]
    public async Task CreateAccountCommandHandler_WhenAccountExists_ShouldThrowAccountHolderConflictException()
    {
        // Arrange
        using var scope = _fixture.Services.CreateScope();
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
        using var scope = _fixture.Services.CreateScope();
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