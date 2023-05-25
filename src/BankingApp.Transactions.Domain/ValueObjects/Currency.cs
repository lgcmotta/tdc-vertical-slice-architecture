using BankingApp.Domain.Core;

namespace BankingApp.Transactions.Domain.ValueObjects;

public sealed class Currency : ValueObject<int, string>
{
    private Currency(int key, string value, string symbol, Money dollarRate) : base(key, value)
    {
        Symbol = symbol;
        DollarRate = dollarRate;
    }

    public string Symbol { get; }
    public Money DollarRate { get; }

    public static Currency BrazilianReal => new(0, "BRL", "R$", 5.00m);
    public static Currency Dollar => new(1, "USD", "$", 1.00m);
    public static Currency Euro => new(2, "EUR", "€", 1.08m);
    public static Currency BritishPound => new(3, "GBP", "£", 1.24m);
    public static Currency UruguayanPeso => new(4, "UYU", "$", 38.64m);
}