using BankingApp.Domain.Core;
using BankingApp.Fees.Domain.ValueObjects;

// ReSharper disable ClassNeverInstantiated.Global
namespace BankingApp.Fees.Domain.Entities;

public sealed class FeeHistory : IEntity<Guid>, ICreatableEntity
{
    public Guid Id { get; set; }

    public Money Amount { get; set; } = Money.Zero;

    public FeeType Type { get; set; }

    public DateTime CreatedAt { get; set; }

    public void SetCreationDateTime(DateTime createdAt)
    {
        CreatedAt = createdAt;
    }
}