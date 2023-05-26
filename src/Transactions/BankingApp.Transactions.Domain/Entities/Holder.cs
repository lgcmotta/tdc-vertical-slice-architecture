using BankingApp.Domain.Core;

namespace BankingApp.Transactions.Domain.Entities;

public sealed class Holder : IEntity<Guid>, ICreatable, IModifiable
{
    private Holder()
    { }

    public Holder(string name, string document, string token) : this()
    {
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
}