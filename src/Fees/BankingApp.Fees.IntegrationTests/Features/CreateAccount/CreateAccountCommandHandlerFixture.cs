// ReSharper disable ClassNeverInstantiated.Global
namespace BankingApp.Fees.IntegrationTests.Features.CreateAccount;

public class CreateAccountCommandHandlerFixture
{
    private readonly Faker<CreateAccountCommand> _commandFaker;
    private readonly Faker<Account> _accountFaker;

    public CreateAccountCommandHandlerFixture()
    {
        _commandFaker = new Faker<CreateAccountCommand>();
        _accountFaker = new Faker<Account>();
    }

    public Account CreateAccount()
    {
        return _accountFaker
            .RuleFor(account => account.Id, faker => faker.Random.Guid())
            .RuleFor(account => account.Token, faker => faker.Finance.Account())
            .Generate();
    }

    public CreateAccountCommand CreateCommand(Guid? holderId = null)
    {
        return _commandFaker
            .CustomInstantiator(faker => new CreateAccountCommand(holderId ?? faker.Random.Guid(), faker.Finance.Account()))
            .Generate();
    }
}