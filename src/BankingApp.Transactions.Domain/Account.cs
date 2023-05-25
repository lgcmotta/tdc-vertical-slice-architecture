using BankingApp.Domain.Core;
using BankingApp.Transactions.Domain.Entities;
using BankingApp.Transactions.Domain.Events;
using BankingApp.Transactions.Domain.Exceptions;
using BankingApp.Transactions.Domain.ValueObjects;
using System.Collections.ObjectModel;

namespace BankingApp.Transactions.Domain;

public sealed class Account : AggregateRoot<Guid>, IModifiable
{
    private List<Transaction> _transactions = new();
    private DateTime _modifiedAt;

    private Account()
    { }

    public Account(string name, string document, string token, Currency currency) : this()
    {
        if (string.IsNullOrWhiteSpace(name)) throw new ArgumentNullException(nameof(name));
        if (string.IsNullOrWhiteSpace(document)) throw new ArgumentNullException(nameof(document));
        if (string.IsNullOrWhiteSpace(token)) throw new ArgumentNullException(nameof(token));

        Owner = new Owner(name, document, token);
        Currency = currency ?? throw new ArgumentNullException(nameof(currency));
        Balance = Money.Zero;
    }

    public Owner Owner { get; private set; }
    public Money Balance { get; private set; }
    public Currency Currency { get; private set; }
    public IEnumerable<Transaction> Transactions => new ReadOnlyCollection<Transaction>(_transactions);

    public string GetFormattedCurrentBalance()
    {
        return Balance.Format(Currency);
    }

    public decimal GetCurrentBalance()
    {
        return Balance.Value;
    }

    public IEnumerable<Transaction> GetBalanceStatementByPeriod(DateTime start, DateTime end)
    {
        start = StartOfDay(start);
        end = EndOfDay(end);

        var transactions = _transactions.Where(transaction => transaction.Occurence >= start && transaction.Occurence <= end);

        return transactions;
    }

    public void Deposit(Money amount, Currency currency, DateTime transactionDateTime)
    {
        if (amount <= Money.Zero)
        {
            throw new InvalidTransactionValueException("Deposit amount must be greater than zero.");
        }

        var currentBalance = Balance;

        var usd = Credit(amount, currency);

        _transactions.Add(new Transaction(usd, currentBalance, TransactionType.Deposit, Id, Id, transactionDateTime));

        AddBalanceChangedDomainEvent(transactionDateTime);
    }

    public void Withdraw(Money amount, DateTime transactionDateTime)
    {
        if (amount <= Money.Zero)
        {
            throw new InvalidTransactionValueException("Withdraw amount must be greater than zero.");
        }

        var currentBalance = Balance;

        var usd = Debit(amount, Currency);

        _transactions.Add(new Transaction(usd, currentBalance, TransactionType.Withdraw, Id, Id, transactionDateTime));

        AddBalanceChangedDomainEvent(transactionDateTime);
    }

    public void Transfer(Money amount, Account receiver, DateTime transactionDateTime)
    {
        if (amount <= Money.Zero)
        {
            throw new InvalidTransactionValueException("Transfer amount must be greater than zero.");
        }

        var usd = SendTransfer(amount, receiver, transactionDateTime);

        receiver.ReceiveTransfer(usd, this, transactionDateTime);

        AddBalanceChangedDomainEvent(transactionDateTime);
        receiver.AddBalanceChangedDomainEvent(transactionDateTime);
    }

    public void ApplyEarnings(Money earnings, DateTime transactionDateTime)
    {
        if (earnings <= Money.Zero)
        {
            throw new InvalidTransactionValueException("Earnings must be greater than zero.");
        }

        var currentBalance = Balance;

        var usd = Earn(earnings);

        _transactions.Add(new Transaction(usd, currentBalance, TransactionType.Earnings, Id, Id, transactionDateTime));
    }

    public void ChangeCurrency(Currency currency)
    {
        if (currency != Currency)
        {
            Currency = currency;
        }
    }

    public void LastModifiedAt(DateTime modifiedAt)
    {
        _modifiedAt = modifiedAt;
    }

    private void AddBalanceChangedDomainEvent(DateTime transactionDateTime)
    {
        AddDomainEvent(new AccountBalanceChangedDomainEvent(Balance, transactionDateTime));
    }

    private Money SendTransfer(Money amount, Account receiver, DateTime transactionDateTime)
    {
        var usd = Debit(amount, Currency);

        _transactions.Add(new Transaction(usd, Balance, TransactionType.TransferOut, Id,  receiver.Id, transactionDateTime));

        return usd;
    }

    private void ReceiveTransfer(Money amount, Account sender, DateTime transactionDateTime)
    {
        Credit(amount, sender.Currency);

        _transactions.Add(new Transaction(amount, Balance, TransactionType.TransferOut, sender.Id,  Id, transactionDateTime));
    }

    private Money Debit(Money amount, Currency currency)
    {
        var usd = amount * currency.DollarRate;

        Balance = new Money(Balance - usd);

        return usd;
    }

    private Money Credit(Money amount, Currency currency)
    {
        var usd = amount / currency.DollarRate;

        Balance = new Money(Balance + usd);

        return usd;
    }

    private Money Earn(Money amount)
    {
        var usd = amount * Currency.Dollar.DollarRate;

        Balance = new Money(Balance + usd);

        return usd;
    }

    private static DateTime StartOfDay(DateTime dateTime) =>
        new(dateTime.Year, dateTime.Month, dateTime.Day, 0, 0, 0, 0);

    private static DateTime EndOfDay(DateTime dateTime) =>
        new(dateTime.Year, dateTime.Month, dateTime.Day, 23, 59, 59, 999);
}