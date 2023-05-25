using BankingApp.Domain.Core;

namespace BankingApp.Accounts.Domain.Entities;

public sealed class AccountHolderInfo : IModifiable
{
    private AccountHolderInfo()
    { }

    public AccountHolderInfo(string name, string document, string email, string phoneNumber) : this()
    {
        if (string.IsNullOrWhiteSpace(name)) throw new ArgumentNullException(nameof(name));
        if (string.IsNullOrWhiteSpace(document)) throw new ArgumentNullException(nameof(document));
        if (string.IsNullOrWhiteSpace(email)) throw new ArgumentNullException(nameof(email));
        if (string.IsNullOrWhiteSpace(phoneNumber)) throw new ArgumentNullException(nameof(phoneNumber));

        Name = name;
        Document = document;
        Email = email;
        PhoneNumber = phoneNumber;
    }

    public string Name { get; private set; }

    public string Document { get; private set; }

    public string Email { get; private set; }

    public string PhoneNumber { get; private set; }

    public DateTime ModifiedAt { get; private set; }

    public void LastModifiedAt(DateTime modifiedAt)
    {
        ModifiedAt = modifiedAt;
    }
}