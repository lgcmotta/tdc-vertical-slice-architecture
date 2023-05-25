using Bogus;
using Bogus.Extensions.Brazil;
using System.Collections;

namespace BankingApp.Transactions.UnitTests.Domain;

using Currency = Currency;

public class AccountFixture
{
    private readonly Faker<Account> _accountFaker;
    private readonly Faker<Money> _moneyFaker;
    private readonly Faker<Currency> _currencyFaker;
    private readonly Faker<DepositData> _depositDataFaker;

    private readonly DateTime _start = new(2023, 5, 1);
    private readonly DateTime _end = new(2023, 5, 31);

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
        private readonly IEnumerable<object[]> _values = new object[][]
        {
            new object[] { null!, "99999999999", "0000000000000" },
            new object[] { "", "99999999999", "0000000000000" },
            new object[] { "Jane Doe", null!, "0000000000000" },
            new object[] { "Jane Doe", "", "0000000000000" },
            new object[] { "Jane Doe", "99999999999", null! },
            new object[] { "Jane Doe", "99999999999", "" },
        };

        public IEnumerator<object[]> GetEnumerator() => _values.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }

    public class InvalidTransactionAmounts : IEnumerable<object[]>
    {
        private readonly IEnumerable<object[]> _values = new []
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