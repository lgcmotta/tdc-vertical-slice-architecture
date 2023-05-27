namespace BankingApp.Transactions.UnitTests.Domain;

using Currency = Currency;

public class AccountFixture
{
    private readonly Faker<Account> _accountFaker;
    private readonly Faker<Money> _moneyFaker;
    private readonly Faker<Currency> _currencyFaker;
    private readonly Faker<DepositData> _depositDataFaker;

    public AccountFixture()
    {
        _accountFaker = new Faker<Account>();
        _moneyFaker = new Faker<Money>();
        _currencyFaker = new Faker<Currency>();
        _depositDataFaker = new Faker<DepositData>();
    }

    public Account GenerateStandardAccount()
    {
        return _accountFaker.CustomInstantiator(faker => new Account(
                faker.Random.Guid(),
                faker.Name.FullName(),
                faker.Person.Cpf(),
                faker.Finance.Account(),
                Currency.Dollar))
            .Generate();
    }

    public Money GenerateMoney()
    {
        return _moneyFaker.CustomInstantiator(fake => new Money(fake.Finance.Amount()))
            .Generate();
    }

    public Money GenerateMoneyBetween(Money left, Money right)
    {
        var min = left >= right ? right : left;
        var max = left <= right ? right : left;
        return _moneyFaker.CustomInstantiator(faker => new Money(faker.Finance.Amount(min, max))).Generate();
    }

    public Money GenerateEarnings()
    {
        return _moneyFaker.CustomInstantiator(faker => new Money(faker.Finance.Amount() * faker.Finance.Random.Decimal())).Generate();
    }

    public Currency PickRandomCurrency()
    {
        return _currencyFaker.CustomInstantiator(faker => faker.PickRandom(Currency.Enumerate<Currency>()))
            .Generate();
    }

    public IEnumerable<DepositData> GenerateDeposits(DateTime start, DateTime end)
    {
        Bogus.DataSets.Date.SystemClock = () => new DateTime(2023, 5, 31);

        return _depositDataFaker.CustomInstantiator(faker => new DepositData(
                GenerateMoney(),
                PickRandomCurrency(),
                faker.Date.Between(start, end)
            ))
            .Generate(3);
    }

    public record DepositData(Money Amount, Currency Currency, DateTime Occurrence);

    public class InvalidAccountConstructorParams : IEnumerable<object[]>
    {
        private readonly IEnumerable<object[]> _values = new[]
        {
            new object[] { Guid.NewGuid(), null!, "99999999999", "0000000000000" },
            new object[] { Guid.NewGuid(), "", "99999999999", "0000000000000" },
            new object[] { Guid.NewGuid(), " ", "99999999999", "0000000000000" },
            new object[] { Guid.NewGuid(),"Jane Doe", null!, "0000000000000" },
            new object[] { Guid.NewGuid(),"Jane Doe", "", "0000000000000" },
            new object[] { Guid.NewGuid(),"Jane Doe", " ", "0000000000000" },
            new object[] { Guid.NewGuid(),"Jane Doe", "99999999999", null! },
            new object[] { Guid.NewGuid(),"Jane Doe", "99999999999", "" },
            new object[] { Guid.NewGuid(),"Jane Doe", "99999999999", " " },
        };

        public IEnumerator<object[]> GetEnumerator() => _values.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }

    public class InvalidTransactionAmounts : IEnumerable<object[]>
    {
        private readonly IEnumerable<object[]> _values = new[]
        {
            new [] { Money.Zero },
            new [] { new Money(-1000.00m) }
        };

        public IEnumerator<object[]> GetEnumerator()
        {
            return _values.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}