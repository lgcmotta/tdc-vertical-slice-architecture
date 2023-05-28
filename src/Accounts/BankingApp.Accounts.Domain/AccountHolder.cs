using BankingApp.Accounts.Domain.Entities;
using BankingApp.Accounts.Domain.Events;
using BankingApp.Accounts.Domain.ValueObjects;
using BankingApp.Domain.Core;
using System.Collections.ObjectModel;

namespace BankingApp.Accounts.Domain;

public class AccountHolder : AggregateRoot<Guid>, ICreatableEntity, IModifiableEntity
{
    private List<AccountToken> _tokens = new();

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
    public IEnumerable<AccountToken> Tokens => new ReadOnlyCollection<AccountToken>(_tokens);
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

    public void AddAccountCreatedDomainEvent()
    {
        var name = $"{FirstName} {LastName}";

        var currentToken = Tokens.SingleOrDefault(token => token.Enabled);

        if (currentToken is null)
        {
            throw new Exception();
        }

        AddDomainEvent(new AccountCreatedDomainEvent(Id, name, Document, currentToken.Value, Currency.Value));
    }

    public string GetCurrentToken()
    {
        var token = _tokens.SingleOrDefault(token => token.Enabled);

        if (token is null)
        {
            throw new Exception();
        }

        return token.Value;
    }
}