using BankingApp.Domain.Core;
using BankingApp.Taxes.Domain.ValueObjects;

namespace BankingApp.Taxes.Domain.Entities;

public class TaxHistory : IEntity<Guid>, ICreatableEntity
{
    public Guid Id { get; set; }

    public Money Amount { get; set; }

    public TaxType Type { get; set; }

    public DateTime CreatedAt { get; set; }

    public void SetCreationDateTime(DateTime createdAt)
    {
        CreatedAt = createdAt;
    }
}