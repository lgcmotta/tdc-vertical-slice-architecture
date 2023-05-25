using BankingApp.Domain.Core;

namespace BankingApp.Accounts.Domain.ValueObjects;

public sealed class TransactionType : ValueObject<int, string>
{
    private TransactionType(int key, string value) : base(key, value)
    { }

    public static TransactionType Deposit => new(0, nameof(Deposit));
    public static TransactionType Withdraw => new(1, nameof(Withdraw));
    public static TransactionType Payment => new(2, nameof(Payment));
    public static TransactionType Transfer => new(3, nameof(Transfer));
    public static TransactionType Earnings => new(4, nameof(Earnings));
}