// ReSharper disable ClassNeverInstantiated.Global
namespace BankingApp.Accounts.UnitTests.Domain;

public sealed class AccountHolderFixture
{
    private readonly Faker<AccountHolder> _accountHolderFaker;
    private readonly Faker<Currency> _currencyFaker;
    private readonly Faker _generalPurposeFaker;

    public AccountHolderFixture()
    {
        _accountHolderFaker = new Faker<AccountHolder>();
        _currencyFaker = new Faker<Currency>();
        _generalPurposeFaker = new Faker();
    }

    public AccountHolder GenerateAccountHolder()
    {
        return _accountHolderFaker.CustomInstantiator(faker => new AccountHolder(
                faker.Name.FirstName(),
                faker.Name.LastName(),
                faker.Person.Cpf(),
                faker.Random.Guid().ToString(),
                PickRandomCurrency()))
            .Generate();
    }

    public Currency PickRandomCurrency()
    {
        return _currencyFaker.CustomInstantiator(faker => faker.PickRandom(Currency.Enumerate<Currency>()))
            .Generate();
    }

    public string GenerateToken()
    {
        return Guid.NewGuid().ToString().Replace("-", string.Empty);
    }

    public string GenerateFirstName()
    {
        return _generalPurposeFaker.Name.FirstName();
    }

    public string GenerateLastName()
    {
        return _generalPurposeFaker.Name.LastName();
    }

    public string GenerateDocument()
    {
        return _generalPurposeFaker.Person.Cpf();
    }

    public class InvalidAccountConstructorParams : IEnumerable<object[]>
    {
        private static readonly string Token = Guid.NewGuid().ToString().Replace("-", string.Empty);

        private readonly IEnumerable<object[]> _values = new[]
        {
            new object[] { null!, "Doe", "999999999", Token },
            new object[] { "", "Doe", "999999999", Token },
            new object[] { " ", "Doe", "999999999", Token },
            new object[] { "John", null!, "999999999", Token },
            new object[] { "John", "", "999999999", Token },
            new object[] { "John", " ", "999999999", Token },
            new object[] { "John", "Doe", null!, Token },
            new object[] { "John", "Doe", "", Token },
            new object[] { "John", "Doe", " ", Token },
            new object[] { "John", "Doe", "999999999", null! },
            new object[] { "John", "Doe", "999999999", "" },
            new object[] { "John", "Doe", "999999999", " " }
        };

        public IEnumerator<object[]> GetEnumerator() => _values.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }


}