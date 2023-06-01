// ReSharper disable ClassNeverInstantiated.Global
namespace BankingApp.Fees.IntegrationTests.Features.OverdraftFee;

public class OverdraftFeeCommandHandlerFixture
{
    private readonly Faker<OverdraftFeeCommand> _commandFaker;
    private readonly Faker<Account> _accountFaker;

    public OverdraftFeeCommandHandlerFixture()
    {
        _commandFaker = new Faker<OverdraftFeeCommand>();
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

    public OverdraftFeeCommand CreateCommand(decimal rate)
    {
        return _commandFaker
            .CustomInstantiator(_ => new OverdraftFeeCommand(rate))
            .Generate();
    }
}