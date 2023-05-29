using BankingApp.Accounts.Domain;
using BankingApp.Accounts.Domain.Entities;
using BankingApp.Accounts.Domain.Exceptions;
using BankingApp.Accounts.Domain.ValueObjects;
using Bogus;
using Bogus.Extensions.Brazil;
using FluentAssertions;
using System.Collections;
using System.Collections.ObjectModel;
using System.Reflection;

namespace BankingApp.Accounts.UnitTests.Domain;

public class AccountHolderTests : IClassFixture<AccountHolderFixture>
{
    private readonly AccountHolderFixture _fixture;

    public AccountHolderTests(AccountHolderFixture fixture)
    {
        _fixture = fixture;
    }

    [Theory]
    [ClassData(typeof(AccountHolderFixture.InvalidAccountConstructorParams))]
    public void AccountHolder_WhenConstructorArgumentsAreNullEmptyOrWhiteSpace_ShouldThrowArgumentNullException(string firstName, string lastName, string document, string token)
    {
        // Arrange
        AccountHolder CreateAccountHolder()
        {
            return new AccountHolder(firstName, lastName, document, token, Currency.Dollar);
        }

        // Act & Assert
        Assert.Throws<ArgumentNullException>(CreateAccountHolder);
    }

    [Fact]
    public void AccountHolder_WhenSettingCreationDateTime_ShouldHaveAccountHolderWithCreationDateTime()
    {
        // Arrange
        var account = _fixture.GenerateAccountHolder();
        var createdAt = new DateTime(2023, 05, 29, 0, 0, 0, 0, 0);

        // Act
        account.SetCreationDateTime(createdAt);

        // Assert
        account.CreatedAt.Should().Be(createdAt);
    }

    [Fact]
    public void AccountHolder_WhenSettingModificationDateTime_ShouldHaveAccountHolderWithModificationDateTime()
    {
        // Arrange
        var account = _fixture.GenerateAccountHolder();
        var modifiedAt = new DateTime(2023, 05, 29, 0, 0, 0, 0, 0);

        // Act
        account.SetModificationDateTime(modifiedAt);

        // Assert
        account.ModifiedAt.Should().Be(modifiedAt);
    }

    [Fact]
    public void AccountHolder_WhenChangingToken_ShouldDisablePreviousTokenAndHaveNewTokenEnabled()
    {
        // Arrange
        var account = _fixture.GenerateAccountHolder();
        var currentToken = account.GetCurrentToken();
        var disabledAt = new DateTime(2023, 05, 29, 0, 0, 0, 0, 0);
        var newToken = _fixture.GenerateToken();

        // Act
        account.ChangeToken(newToken, disabledAt);

        var disabledToken = account.Tokens.Single(token => !token.Enabled);
        var enabledToken = account.Tokens.Single(token => token.Enabled);

        // Assert
        disabledToken.Value.Should().Be(currentToken);
        disabledToken.DisabledAt.Should().Be(disabledAt);
        enabledToken.Value.Should().Be(newToken);
        enabledToken.DisabledAt.Should().BeNull();
    }

    [Fact]
    public void AccountHolder_WhenNoTokenIsEnabled_ShouldThrowAccountHolderCurrentTokenNotFoundException()
    {
        // Arrange
        var account = _fixture.GenerateAccountHolder();

        var tokens = typeof(AccountHolder)
            .GetField("_tokens", BindingFlags.Instance | BindingFlags.NonPublic)!
            .GetValue(account) as List<AccountToken>;

        tokens!.Clear();

        // Act & Assert
        Assert.Throws<AccountHolderCurrentTokenNotFound>(account.GetCurrentToken);
    }

    [Fact]
    public void AccountHolder_WhenNewFirstNameIsEqualToCurrentFirstName_ShouldNotChangeFirstName()
    {
        // Arrange
        var account = _fixture.GenerateAccountHolder();
        var firstName = account.FirstName;

        // Act
        account.CorrectFirstName(firstName);

        // Assert
        account.FirstName.Should().Be(firstName);
    }

    [Fact]
    public void AccountHolder_WhenNewFirstNameNotIsEqualToCurrentFirstName_ShouldChangeFirstName()
    {
        // Arrange
        var account = _fixture.GenerateAccountHolder();
        var firstName = _fixture.GenerateFirstName();

        // Act
        account.CorrectFirstName(firstName);

        // Assert
        account.FirstName.Should().Be(firstName);
    }

    [Fact]
    public void AccountHolder_WhenNewLastNameIsEqualToCurrentLastName_ShouldNotChangeLastName()
    {
        // Arrange
        var account = _fixture.GenerateAccountHolder();
        var lastName = account.LastName;

        // Act
        account.CorrectLastName(lastName);

        // Assert
        account.LastName.Should().Be(lastName);
    }

    [Fact]
    public void AccountHolder_WhenNewLastNameNotIsEqualToCurrentLastName_ShouldChangeLastName()
    {
        // Arrange
        var account = _fixture.GenerateAccountHolder();
        var lastName = _fixture.GenerateLastName();

        // Act
        account.CorrectLastName(lastName);

        // Assert
        account.LastName.Should().Be(lastName);
    }

    [Fact]
    public void AccountHolder_WhenNewDocumentIsEqualToCurrentDocument_ShouldNotChangeDocument()
    {
        // Arrange
        var account = _fixture.GenerateAccountHolder();
        var document = account.Document;

        // Act
        account.ChangeDocument(document);

        // Assert
        account.Document.Should().Be(document);
    }

    [Fact]
    public void AccountHolder_WhenNewDocumentNotIsEqualToCurrentDocument_ShouldChangeDocument()
    {
        // Arrange
        var account = _fixture.GenerateAccountHolder();
        var document = _fixture.GenerateDocument();

        // Act
        account.ChangeDocument(document);

        // Assert
        account.Document.Should().Be(document);
    }

    [Fact]
    public void AccountHolder_WhenNewCurrencyIsEqualToCurrentCurrency_ShouldNotChangeCurrency()
    {
        // Arrange
        var account = _fixture.GenerateAccountHolder();
        var currency = account.Currency;

        // Act
        account.ChangeCurrency(currency);

        // Assert
        account.Currency.Should().Be(currency);
    }

    [Fact]
    public void AccountHolder_WhenNewCurrencyNotIsEqualToCurrentCurrency_ShouldChangeCurrency()
    {
        // Arrange
        var account = _fixture.GenerateAccountHolder();
        var currency = _fixture.PickRandomCurrency();

        // Act
        account.ChangeCurrency(currency);

        // Assert
        account.Currency.Should().Be(currency);
    }

    [Fact]
    public void AccountHolder_WhenAddingAccountCreatedDomainEvent_ShouldHaveDomainEventCountAsOne()
    {
        // Arrange
        var account = _fixture.GenerateAccountHolder();

        // Act
        account.AddAccountCreatedDomainEvent();

        // Assert
        account.DomainEvents.Should().NotBeEmpty().And.HaveCount(1);
    }

    [Fact]
    public void AccountHolder_WhenAddingAccountUpdatedDomainEvent_ShouldHaveDomainEventCountAsOne()
    {
        // Arrange
        var account = _fixture.GenerateAccountHolder();

        // Act
        account.AddAccountUpdatedDomainEvent();

        // Assert
        account.DomainEvents.Should().NotBeEmpty().And.HaveCount(1);
    }

    [Fact]
    public void AccountHolder_WhenAddingAccountPartiallyUpdatedDomainEvent_ShouldHaveDomainEventCountAsOne()
    {
        // Arrange
        var account = _fixture.GenerateAccountHolder();

        // Act
        account.AddAccountPatchedDomainEvent();

        // Assert
        account.DomainEvents.Should().NotBeEmpty().And.HaveCount(1);
    }

    [Fact]
    public void AccountHolder_WhenAddingAccountTokenChangedDomainEvent_ShouldHaveDomainEventCountAsOne()
    {
        // Arrange
        var account = _fixture.GenerateAccountHolder();

        // Act
        account.AddAccountTokenChangedDomainEvent();

        // Assert
        account.DomainEvents.Should().NotBeEmpty().And.HaveCount(1);
    }
}

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