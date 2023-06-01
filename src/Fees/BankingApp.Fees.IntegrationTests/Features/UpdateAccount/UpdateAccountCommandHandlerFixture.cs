// ReSharper disable ClassNeverInstantiated.Global
namespace BankingApp.Fees.IntegrationTests.Features.UpdateAccount;

public class UpdateAccountCommandHandlerFixture
{
    private readonly Faker<UpdateAccountCommand> _commandFaker;
    private readonly Faker<Account> _accountFaker;

    public UpdateAccountCommandHandlerFixture()
    {
        _commandFaker = new Faker<UpdateAccountCommand>();
        _accountFaker = new Faker<Account>();
    }

    public Account CreateAccount()
    {
        return _accountFaker
            .RuleFor(account => account.Id, faker => faker.Random.Guid())
            .RuleFor(account => account.Token, faker => faker.Finance.Account())
            .Generate();
    }

    public UpdateAccountCommand CreateCommand(Guid? holderId = null)
    {
        return _commandFaker
            .CustomInstantiator(faker => new UpdateAccountCommand(holderId ?? faker.Random.Guid(), faker.Finance.Account()))
            .Generate();
    }
}