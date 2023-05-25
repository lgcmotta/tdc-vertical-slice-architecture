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

    public Account(string token, Currency currency) : this()
    {
        if (string.IsNullOrWhiteSpace(token)) throw new ArgumentNullException(nameof(token));

        Token = token;
        Currency = currency ?? throw new ArgumentNullException(nameof(currency));
        Balance = Money.Zero;
    }

    public string Token { get; private set; }
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

        var usd = amount / currency.DollarRate;

        _transactions.Add(new Transaction(usd, Balance, TransactionType.Deposit, transactionDateTime));

        Balance = new Money(Balance + usd);

        AddBalanceChangedDomainEvent(transactionDateTime);
    }

    public void Withdraw(Money amount, DateTime transactionDateTime)
    {
        if (amount <= Money.Zero)
        {
            throw new InvalidTransactionValueException("Withdraw amount must be greater than zero.");
        }

        var usd = amount * Currency.DollarRate;

        _transactions.Add(new Transaction(usd, Balance, TransactionType.Withdraw, transactionDateTime));

        Balance = new Money(Balance - usd);

        AddBalanceChangedDomainEvent(transactionDateTime);
    }

    public void Transfer(Money amount, DateTime transactionDateTime)
    {
        if (amount <= Money.Zero)
        {
            throw new InvalidTransactionValueException("Transfer amount must be greater than zero.");
        }

        var usd = amount * Currency.DollarRate;

        _transactions.Add(new Transaction(usd, Balance, TransactionType.Transfer, transactionDateTime));

        Balance = new Money(Balance - usd);

        AddBalanceChangedDomainEvent(transactionDateTime);
    }

    public void ApplyEarnings(Money earnings, DateTime transactionDateTime)
    {
        if (earnings <= Money.Zero)
        {
            throw new InvalidTransactionValueException("Earnings must be greater than zero.");
        }

        var usd = earnings * Currency.Dollar.DollarRate;

        _transactions.Add(new Transaction(usd, Balance, TransactionType.Earnings, transactionDateTime));

        Balance = new Money(Balance * usd);
    }

    public void ChangeCurrency(Currency currency)
    {
        if (currency != Currency)
        {
            Currency = currency;
        }
    }

    private void AddBalanceChangedDomainEvent(DateTime transactionDateTime)
    {
        AddDomainEvent(new AccountBalanceChangedDomainEvent(Balance, transactionDateTime));
    }

    private static DateTime StartOfDay(DateTime dateTime) =>
        new(dateTime.Year, dateTime.Month, dateTime.Day, 0, 0, 0, 0);

    private static DateTime EndOfDay(DateTime dateTime) =>
        new(dateTime.Year, dateTime.Month, dateTime.Day, 23, 59, 59, 999);

    public void LastModifiedAt(DateTime modifiedAt)
    {
        _modifiedAt = modifiedAt;
    }
}