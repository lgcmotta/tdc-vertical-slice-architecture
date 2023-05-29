using BankingApp.Accounts.Domain.Entities;
using BankingApp.Accounts.Domain.Events;
using BankingApp.Accounts.Domain.Exceptions;
using BankingApp.Accounts.Domain.ValueObjects;
using BankingApp.Domain.Core;

namespace BankingApp.Accounts.Domain;

public class AccountHolder : AggregateRoot<Guid>, ICreatableEntity, IModifiableEntity
{
    private readonly List<AccountToken> _tokens = new();

    private AccountHolder()
    { }

    public AccountHolder(string firstName, string lastName, string document, string token, Currency currency) : this()
    {
        if (string.IsNullOrWhiteSpace(firstName)) throw new ArgumentNullException(nameof(firstName));
        if (string.IsNullOrWhiteSpace(lastName)) throw new ArgumentNullException(nameof(lastName));
        if (string.IsNullOrWhiteSpace(document)) throw new ArgumentNullException(nameof(document));
        if (string.IsNullOrWhiteSpace(token)) throw new ArgumentNullException(nameof(token));

        _tokens.Add(new AccountToken(token));

        Id = Guid.NewGuid();
        FirstName = firstName;
        LastName = lastName;
        Document = document;
        Currency = currency;
        ModifiedAt = null;
    }

    public string FirstName { get; private set; }
    public string LastName { get; private set; }
    public string Document { get; private set; }
    public Currency Currency { get; private set; }
    public IEnumerable<AccountToken> Tokens => _tokens.AsReadOnly();
    public DateTime CreatedAt { get; private set; }
    public DateTime? ModifiedAt { get; private set; }

    public void SetCreationDateTime(DateTime createdAt)
    {
        CreatedAt = createdAt;
    }

    public void SetModificationDateTime(DateTime modifiedAt)
    {
        ModifiedAt = modifiedAt;
    }

    public void ChangeToken(string value, DateTime currentTokenDisabledAt)
    {
        var currentToken = Tokens.SingleOrDefault(token => token.Enabled);

        currentToken?.Disable(currentTokenDisabledAt);

        var token = new AccountToken(value);

        _tokens.Add(token);
    }

    public string GetCurrentToken()
    {
        var token = _tokens.SingleOrDefault(token => token.Enabled);

        if (token is null)
        {
            throw new AccountHolderCurrentTokenNotFound("Current token not found");
        }

        return token.Value;
    }

    public void CorrectFirstName(string firstName)
    {
        if (FirstName == firstName) return;

        FirstName = firstName;
    }

    public void CorrectLastName(string lastName)
    {
        if (LastName == lastName) return;

        LastName = lastName;
    }

    public void ChangeDocument(string document)
    {
        if (Document == document) return;

        Document = document;
    }

    public void ChangeCurrency(Currency currency)
    {
        if (Currency == currency) return;

        Currency = currency;
    }

    public void AddAccountCreatedDomainEvent()
    {
        var name = $"{FirstName} {LastName}";

        var currentToken = GetCurrentToken();

        AddDomainEvent(new AccountCreatedDomainEvent(Id, name, Document, currentToken, Currency.Value));
    }

    public void AddAccountUpdatedDomainEvent()
    {
        var name = $"{FirstName} {LastName}";

        var currentToken = GetCurrentToken();

        AddDomainEvent(new AccountUpdatedDomainEvent(Id, name, currentToken, Currency.Value));
    }

    public void AddAccountPartiallyUpdatedDomainEvent()
    {
        var name = $"{FirstName} {LastName}";

        var currentToken = GetCurrentToken();

        AddDomainEvent(new AccountPartiallyUpdatedDomainEvent(Id, name, currentToken, Currency.Value));
    }

    public void AddAccountTokenChangedDomainEvent()
    {
        var currentToken = GetCurrentToken();

        AddDomainEvent(new AccountTokenChangedDomainEvent(Id, currentToken));
    }
}