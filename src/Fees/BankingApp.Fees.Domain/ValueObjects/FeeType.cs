using BankingApp.Domain.Core;

namespace BankingApp.Fees.Domain.ValueObjects;

public class FeeType : ValueObject<int, string>
{
    private FeeType(int key, string value) : base(key, value)
    { }

    public static FeeType Overdraft => new(0, nameof(Overdraft));

    public static FeeType Profit => new(1, nameof(Profit));
}