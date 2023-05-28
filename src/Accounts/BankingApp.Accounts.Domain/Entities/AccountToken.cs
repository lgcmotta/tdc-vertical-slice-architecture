using BankingApp.Domain.Core;

namespace BankingApp.Accounts.Domain.Entities;

public class AccountToken : IEntity<Guid>, ICreatableEntity
{
    private AccountToken()
    { }

    public AccountToken(string value) : this()
    {
        Value = value;
        Enabled = true;
        DisabledAt = null;
    }

    public Guid Id { get; private set; }
    public string Value { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public bool Enabled { get; private set; }
    public DateTime? DisabledAt { get; private set; }

    public void SetCreationDateTime(DateTime createdAt)
    {
        CreatedAt = createdAt;
    }

    public void Disable(DateTime disabledAt)
    {
        Enabled = false;
        DisabledAt = disabledAt;
    }
}