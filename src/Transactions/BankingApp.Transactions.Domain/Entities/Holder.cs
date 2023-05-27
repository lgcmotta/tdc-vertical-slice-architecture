using BankingApp.Domain.Core;

namespace BankingApp.Transactions.Domain.Entities;

public sealed class Holder : IEntity<Guid>, ICreatableEntity, IModifiableEntity
{
    private Holder()
    { }

    public Holder(Guid id, string name, string document, string token) : this()
    {
        Id = id;
        Name = name;
        Document = document;
        Token = token;
    }

    public Guid Id { get; private set;  }
    public string Name { get; private set; }
    public string Document { get; private set; }
    public string Token { get; private set; }
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

    public void ChangeName(string name)
    {
        if (Name == name) return;

        Name = name;
    }

    public void ChangeDocument(string document)
    {
        if (Document == document) return;

        Document = document;
    }

    public void UpdateToken(string token)
    {
        if (Token == token) return;

        Token = token;
    }
}