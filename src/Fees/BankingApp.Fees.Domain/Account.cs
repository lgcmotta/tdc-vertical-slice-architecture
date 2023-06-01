using BankingApp.Domain.Core;
using BankingApp.Fees.Domain.Entities;
using BankingApp.Fees.Domain.ValueObjects;

namespace BankingApp.Fees.Domain;

public class Account : AggregateRoot<Guid>
{
    public new Guid Id { get; set; }

    public string Token { get; set; } = string.Empty;

    public Money CurrentBalanceInUSD { get; set; } = Money.Zero;

    public DateTime LastBalanceChange { get; set; }

    public List<FeeHistory> FeeHistory { get; set; } = new();
}