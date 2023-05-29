using BankingApp.Domain.Core;

namespace BankingApp.Accounts.Domain.ValueObjects;

public sealed class Currency : ValueObject<int, string>
{
    private Currency(int key, string value) : base(key, value)
    { }

    public static Currency BrazilianReal => new(0, "BRL");
    public static Currency Dollar => new(1, "USD");
    public static Currency Euro => new(2, "EUR");
    public static Currency BritishPound => new(3, "GBP");
    public static Currency UruguayanPeso => new(4, "UYU");
}