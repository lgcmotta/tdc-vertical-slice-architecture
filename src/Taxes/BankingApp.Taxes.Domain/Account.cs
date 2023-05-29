using BankingApp.Domain.Core;
using BankingApp.Taxes.Domain.Entities;
using BankingApp.Taxes.Domain.ValueObjects;

namespace BankingApp.Taxes.Domain;

public class Account : AggregateRoot<Guid>
{
    public new Guid Id { get; set; }

    public string Token { get; set; } = string.Empty;

    public Money CurrentBalanceInUSD { get; set; } = Money.Zero;

    public DateTime LastBalanceChange { get; set; }

    public IEnumerable<FeeHistory> FeeHistory { get; set; } = new List<FeeHistory>();
}