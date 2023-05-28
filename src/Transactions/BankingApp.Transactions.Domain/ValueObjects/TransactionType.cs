using BankingApp.Domain.Core;

namespace BankingApp.Transactions.Domain.ValueObjects;

public sealed class TransactionType : ValueObject<int, string>
{
    private TransactionType(int key, string value) : base(key, value)
    { }

    public static TransactionType Deposit => new(0, nameof(Deposit));
    public static TransactionType Withdraw => new(1, nameof(Withdraw));
    public static TransactionType Payment => new(2, nameof(Payment));
    public static TransactionType TransferIn => new(3, nameof(TransferIn));
    public static TransactionType TransferOut => new(4, nameof(TransferOut));
    public static TransactionType Earnings => new(5, nameof(Earnings));
}