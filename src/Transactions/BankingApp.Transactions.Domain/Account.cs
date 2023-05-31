using BankingApp.Domain.Core;
using BankingApp.Transactions.Domain.Entities;
using BankingApp.Transactions.Domain.Events;
using BankingApp.Transactions.Domain.Exceptions;
using BankingApp.Transactions.Domain.ValueObjects;

namespace BankingApp.Transactions.Domain;

public sealed class Account : AggregateRoot<Guid>, ICreatableEntity, IModifiableEntity
{
    private readonly List<Transaction> _transactions = new();
    private DateTime? _modifiedAt;
    private DateTime _createdAt;

    private Account()
    { }

    public Account(Guid holderId, string name, string document, string token, Currency currency) : this()
    {
        if (string.IsNullOrWhiteSpace(name)) throw new ArgumentNullException(nameof(name));
        if (string.IsNullOrWhiteSpace(document)) throw new ArgumentNullException(nameof(document));
        if (string.IsNullOrWhiteSpace(token)) throw new ArgumentNullException(nameof(token));

        _modifiedAt = null;
        _transactions = new List<Transaction>();
        Id = holderId;
        Name = name;
        Token = token;
        DisplayCurrency = currency ?? throw new ArgumentNullException(nameof(currency));
        BalanceInUSD = Money.Zero;
    }

    public string Name { get; private set; }
    public string Token { get; private set; }
    public Money BalanceInUSD { get; private set; }
    public Currency DisplayCurrency { get; private set; }
    public DateTime CreatedAt => _createdAt;
    public DateTime? ModifiedAt => _modifiedAt;
    public IEnumerable<Transaction> Transactions => _transactions.AsReadOnly();

    public string GetFormattedCurrentBalance()
    {
        return BalanceInUSD.Format(DisplayCurrency);
    }

    public decimal GetCurrentBalance()
    {
        return BalanceInUSD.Value;
    }

    public IEnumerable<Transaction> GetBalanceStatementByPeriod(DateTime start, DateTime end)
    {
        start = StartOfDay(start);
        end = EndOfDay(end);

        var transactions = _transactions.Where(transaction => transaction.Occurence >= start && transaction.Occurence <= end);

        return transactions;
    }

    public Transaction Deposit(Money amount, Currency currency, DateTime transactionDateTime)
    {
        if (amount <= Money.Zero)
        {
            throw new InvalidTransactionValueException("Deposit amount must be greater than zero.");
        }

        var currentBalance = BalanceInUSD;

        var usd = Money.ConvertToUSD(amount, currency);

        Credit(usd);

        var transaction = new Transaction(usd, currentBalance, TransactionType.Deposit, Id, Id, transactionDateTime);

        _transactions.Add(transaction);

        AddDomainEvent(new DepositDomainEvent(Id, BalanceInUSD));

        return transaction;
    }

    public Transaction Withdraw(Money amount, DateTime transactionDateTime)
    {
        if (amount <= Money.Zero)
        {
            throw new InvalidTransactionValueException("Withdraw amount must be greater than zero.");
        }

        var currentBalance = BalanceInUSD;

        var usd = Money.ConvertToUSD(amount, DisplayCurrency);

        Debit(usd);

        var transaction = new Transaction(usd, currentBalance, TransactionType.Withdraw, Id, Id, transactionDateTime);

        _transactions.Add(transaction);

        AddDomainEvent(new WithdrawDomainEvent(Id, BalanceInUSD));

        return transaction;
    }

    public Transaction TransferOut(Guid receiverId, Money amount, Currency currency, DateTime transactionDateTime)
    {
        if (amount <= Money.Zero)
        {
            throw new InvalidTransactionValueException("Transfer amount must be greater than zero.");
        }

        var usd = Money.ConvertToUSD(amount, currency);

        var balanceSnapShot = BalanceInUSD;

        Debit(usd);

        var transaction = new Transaction(usd, balanceSnapShot, TransactionType.TransferOut, Id, receiverId, transactionDateTime);

        _transactions.Add(transaction);

        AddDomainEvent(new TransferOutDomainEvent(Id, BalanceInUSD));

        return transaction;
    }

    public void TransferIn(Guid senderId, Money amount, Currency currency, DateTime transactionDateTime)
    {
        if (amount <= Money.Zero)
        {
            throw new InvalidTransactionValueException("Transfer amount must be greater than zero.");
        }

        var usd = Money.ConvertToUSD(amount, currency);

        var balanceSnapShot = BalanceInUSD;

        Credit(usd);

        var transaction = new Transaction(usd, balanceSnapShot, TransactionType.TransferIn, senderId, Id, transactionDateTime);

        _transactions.Add(transaction);

        AddDomainEvent(new TransferInDomainEvent(Id, BalanceInUSD));
    }
    public void ApplyOverdraftFee(Money overdraftFee, DateTime transactionDateTime)
    {
        var balanceSnapShot = BalanceInUSD;

        Debit(overdraftFee);

        var transaction = new Transaction(overdraftFee, balanceSnapShot, TransactionType.OverdraftFee, Id, Id, transactionDateTime);

        _transactions.Add(transaction);
    }

    public void ApplyEarnings(Money earnings, DateTime transactionDateTime)
    {
        if (earnings <= Money.Zero)
        {
            throw new InvalidTransactionValueException("Earnings must be greater than zero.");
        }

        var currentBalance = BalanceInUSD;

        var usd = Earn(earnings);

        _transactions.Add(new Transaction(usd, currentBalance, TransactionType.Earnings, Id, Id, transactionDateTime));
    }

    public void ChangeCurrency(Currency currency)
    {
        if (currency != DisplayCurrency)
        {
            DisplayCurrency = currency;
        }
    }

    public void ChangeHolderName(string? name)
    {
        if (string.IsNullOrWhiteSpace(name) || Name == name) return;

        Name = name;
    }

    public void UpdateHolderToken(string? token)
    {
        if (string.IsNullOrWhiteSpace(token) || Token == token) return;

        Token = token;
    }

    public void SetModificationDateTime(DateTime modifiedAt)
    {
        _modifiedAt = modifiedAt;
    }

    public void SetCreationDateTime(DateTime createdAt)
    {
        _createdAt = createdAt;
    }

    private void Debit(Money amount)
    {
        BalanceInUSD = new Money(BalanceInUSD - amount);
    }

    private void Credit(Money amount)
    {
        BalanceInUSD = new Money(BalanceInUSD + amount);
    }

    private Money Earn(Money amount)
    {
        var usd = Money.ConvertToUSD(amount, DisplayCurrency);

        BalanceInUSD = new Money(BalanceInUSD + usd);

        return usd;
    }

    private static DateTime StartOfDay(DateTime dateTime) =>
        new(dateTime.Year, dateTime.Month, dateTime.Day, 0, 0, 0, 0);

    private static DateTime EndOfDay(DateTime dateTime) =>
        new(dateTime.Year, dateTime.Month, dateTime.Day, 23, 59, 59, 999);
}