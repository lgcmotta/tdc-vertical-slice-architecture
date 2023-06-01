// ReSharper disable ClassNeverInstantiated.Global
namespace BankingApp.Fees.IntegrationTests.Features.UpdateBalance;

public class UpdateBalanceCommandHandlerFixture
{
    private readonly Faker<UpdateBalanceCommand> _commandFaker;
    private readonly Faker<Account> _accountFaker;

    public UpdateBalanceCommandHandlerFixture()
    {
        _commandFaker = new Faker<UpdateBalanceCommand>();
        _accountFaker = new Faker<Account>();
        Bogus.DataSets.Date.SystemClock = () => new DateTime(2023, 5, 1, 0, 0, 0);
    }

    public Account CreateAccount(decimal? balance = null, DateTime? lastBalanceChange = null)
    {
        return _accountFaker
            .RuleFor(account => account.Id, faker => faker.Random.Guid())
            .RuleFor(account => account.Token, faker => faker.Finance.Account())
            .RuleFor(account => account.CurrentBalanceInUSD, faker => new Money(balance ?? faker.Finance.Amount()))
            .RuleFor(account => account.LastBalanceChange, faker => lastBalanceChange ?? faker.Date.Past())
            .Generate();
    }

    public UpdateBalanceCommand CreateCommand(Guid? holderId = null)
    {
        return _commandFaker
            .CustomInstantiator(faker => new UpdateBalanceCommand(holderId ?? faker.Random.Guid(), faker.Finance.Amount()))
            .Generate();
    }
}