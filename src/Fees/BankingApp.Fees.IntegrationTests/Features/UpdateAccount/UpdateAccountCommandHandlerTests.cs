﻿namespace BankingApp.Fees.IntegrationTests.Features.UpdateAccount;

[Collection("FeesWebApplicationFactory")]
public class UpdateAccountCommandHandlerTests : IClassFixture<UpdateAccountCommandHandlerFixture>
{
    private readonly UpdateAccountCommandHandlerFixture _fixture;
    private readonly FeesWebApplicationFactory _factory;

    public UpdateAccountCommandHandlerTests(UpdateAccountCommandHandlerFixture fixture, FeesWebApplicationFactory factory)
    {
        _fixture = fixture;
        _factory = factory;
    }

    [Fact]
    public async Task UpdateAccountCommandHandler_WhenAccountNotExists_ShouldThrowAccountNotFoundException()
    {
        // Arrange
        using var scope = _factory.Services.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<AccountFeesDbContext>();

        var command = _fixture.CreateCommand();

        var handler = new UpdateAccountCommandHandler(context);

        // Act & Assert
        await Assert.ThrowsAsync<AccountNotFoundException>(() => handler.Handle(command, CancellationToken.None))
            .ConfigureAwait(continueOnCapturedContext: false);
    }

    [Fact]
    public async Task UpdateAccountCommandHandler_WhenAccountExists_ShouldUpdateAccount()
    {
        using var scope = _factory.Services.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<AccountFeesDbContext>();

        var account = _fixture.CreateAccount();

        await context.Accounts.AddAsync(account).ConfigureAwait(continueOnCapturedContext: false);
        await context.SaveChangesAsync().ConfigureAwait(continueOnCapturedContext: false);

        var command = _fixture.CreateCommand(account.Id);

        var handler = new UpdateAccountCommandHandler(context);

        // Act
        await handler.Handle(command, CancellationToken.None).ConfigureAwait(continueOnCapturedContext: false);
        await context.SaveChangesAsync().ConfigureAwait(continueOnCapturedContext: false);

        var updatedAccount = await context.Accounts.FirstOrDefaultAsync(updated => updated.Id == command.HolderId)
            .ConfigureAwait(continueOnCapturedContext: false);

        // Assert
        updatedAccount.Should().NotBeNull();
        updatedAccount!.Token.Should().Be(command.Token);
    }
}