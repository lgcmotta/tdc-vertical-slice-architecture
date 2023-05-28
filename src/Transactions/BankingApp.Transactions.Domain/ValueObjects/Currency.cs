using BankingApp.Domain.Core;

namespace BankingApp.Transactions.Domain.ValueObjects;

public sealed class Currency : ValueObject<int, string>
{
    private Currency(int key, string value, string symbol, Money dollarExchangeRate) : base(key, value)
    {
        Symbol = symbol;
        DollarExchangeRate = dollarExchangeRate;
    }

    public string Symbol { get; }
    public Money DollarExchangeRate { get; }

    public static Currency BrazilianReal => new(0, "BRL", "R$", 5.00m);
    public static Currency Dollar => new(1, "USD", "$", 1.00m);
    public static Currency Euro => new(2, "EUR", "€", 0.93m);
    public static Currency PoundSterling => new(3, "GBP", "£", 0.81m);
    public static Currency UruguayanPeso => new(4, "UYU", "$U", 38.64m);
}