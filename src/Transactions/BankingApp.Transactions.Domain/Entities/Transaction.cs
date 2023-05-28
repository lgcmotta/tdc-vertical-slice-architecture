using BankingApp.Domain.Core;
using BankingApp.Transactions.Domain.ValueObjects;

namespace BankingApp.Transactions.Domain.Entities;

public sealed class Transaction : IEntity<Guid>
{
    private readonly Money _usdValue;
    private readonly Money _balanceInUSDSnapShot;

    private Transaction()
    { }

    public Transaction(
        Money usdValue,
        Money balanceInUSDSnapShot,
        TransactionType type,
        Guid senderId,
        Guid receiverId,
        DateTime occurence) : this()
    {

        _usdValue = usdValue;
        _balanceInUSDSnapShot = balanceInUSDSnapShot;
        Id = Guid.NewGuid();
        Type = type;
        SenderId = senderId;
        ReceiverId = receiverId;
        Occurence = occurence;
    }

    public Guid Id { get; private set; }
    public Guid SenderId { get; private set; }
    public Guid ReceiverId { get; private set; }
    public TransactionType Type { get; private set; }
    public DateTime Occurence { get; private set; }

    public Money USDValue => IsCreditTransaction()
        ? _usdValue
        : _usdValue.Negative();


    // ReSharper disable once ConvertToAutoPropertyWhenPossible
    internal Money PureUSDValue => _usdValue;

    private bool IsCreditTransaction() => Type == TransactionType.Deposit ||
                                          Type == TransactionType.Earnings ||
                                          Type == TransactionType.TransferIn;

    public Money GetBalanceBeforeTransaction() => _balanceInUSDSnapShot;
}