using BankingApp.Domain.Core;
using BankingApp.Transactions.Domain.ValueObjects;

namespace BankingApp.Transactions.Domain.Entities;

public sealed class Transaction : IEntity<Guid>
{
    private readonly Money _value;
    private readonly Money _balanceSnapShot;

    private Transaction()
    { }

    public Transaction(Money value, Money balanceSnapShot, TransactionType type, DateTime occurence) : this()
    {
        _value = value;
        _balanceSnapShot = balanceSnapShot;
        Type = type;
        Occurence = occurence;
    }

    public Guid Id { get; private set; }
    public TransactionType Type { get; private set; }
    public DateTime Occurence { get; private set; }

    internal Money Value => IsCreditTransaction()
        ? _value
        : _value.Negative();

    private bool IsCreditTransaction() => Type == TransactionType.Deposit || Type == TransactionType.Earnings;

    internal Money GetBalanceBeforeTransaction() => _balanceSnapShot;
}