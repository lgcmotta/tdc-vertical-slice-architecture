using BankingApp.Domain.Core;

namespace BankingApp.Transactions.Domain.Entities;

public sealed class Owner : IModifiable
{
    private Owner()
    { }

    public Owner(string name, string document, string token) : this()
    {
        Name = name;
        Document = document;
        Token = token;
    }

    public string Name { get; private set; }

    public string Document { get; private set; }

    public string Token { get; private set; }

    public DateTime ModifiedAt { get; private set; }

    public void LastModifiedAt(DateTime modifiedAt)
    {
        ModifiedAt = modifiedAt;
    }
}