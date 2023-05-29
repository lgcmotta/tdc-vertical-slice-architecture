using BankingApp.Domain.Core;

namespace BankingApp.Taxes.Domain.ValueObjects;

public class TaxType : ValueObject<int, string>
{
    private TaxType(int key, string value) : base(key, value)
    { }

    public static TaxType Fee => new(0, nameof(Fee));

    public static TaxType Income => new(1, nameof(Income));
}