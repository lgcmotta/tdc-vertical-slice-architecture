﻿using BankingApp.Domain.Core;
using BankingApp.Transactions.Domain.Entities;
using BankingApp.Transactions.Domain.Events;
using BankingApp.Transactions.Domain.Exceptions;
using BankingApp.Transactions.Domain.ValueObjects;
using System.Collections.ObjectModel;

namespace BankingApp.Transactions.Domain;

public sealed class Account : AggregateRoot<Guid>, ICreatableEntity, IModifiableEntity
{
    private List<Transaction> _transactions = new();
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
        Holder = new Holder(holderId, name, document, token);
        DisplayCurrency = currency ?? throw new ArgumentNullException(nameof(currency));
        BalanceInUSD = Money.Zero;
    }

    public Holder Holder { get; private set; }
    public Money BalanceInUSD { get; private set; }
    public Currency DisplayCurrency { get; private set; }
    public DateTime CreatedAt => _createdAt;
    public DateTime? ModifiedAt => _modifiedAt;
    public IEnumerable<Transaction> Transactions => new ReadOnlyCollection<Transaction>(_transactions);

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

        var usd = ConvertToUSD(amount, currency);

        Credit(usd);

        var transaction = new Transaction(usd, currentBalance, TransactionType.Deposit, Id, Id, transactionDateTime);

        _transactions.Add(transaction);

        AddBalanceChangedDomainEvent(transactionDateTime);

        return transaction;
    }

    public Transaction Withdraw(Money amount, DateTime transactionDateTime)
    {
        if (amount <= Money.Zero)
        {
            throw new InvalidTransactionValueException("Withdraw amount must be greater than zero.");
        }

        var currentBalance = BalanceInUSD;

        var usd = ConvertToUSD(amount, DisplayCurrency);

        Debit(usd);

        var transaction = new Transaction(usd, currentBalance, TransactionType.Withdraw, Id, Id, transactionDateTime);

        _transactions.Add(transaction);

        AddBalanceChangedDomainEvent(transactionDateTime);

        return transaction;
    }

    public Transaction Transfer(Money amount, Currency currency, Account receiver, DateTime transactionDateTime)
    {
        if (amount <= Money.Zero)
        {
            throw new InvalidTransactionValueException("Transfer amount must be greater than zero.");
        }

        var usd = ConvertToUSD(amount, currency);

        var sendTransaction = SendTransfer(usd, receiver, transactionDateTime);

        receiver.ReceiveTransfer(sendTransaction.PureUSDValue , this, transactionDateTime);

        AddBalanceChangedDomainEvent(transactionDateTime);
        receiver.AddBalanceChangedDomainEvent(transactionDateTime);

        return sendTransaction;
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
        if (string.IsNullOrWhiteSpace(name)) return;

        Holder.ChangeName(name);
    }

    public void ChangeHolderDocument(string? document)
    {
        if (string.IsNullOrWhiteSpace(document)) return;

        Holder.ChangeDocument(document);
    }

    public void UpdateHolderToken(string? token)
    {
        if (string.IsNullOrWhiteSpace(token)) return;

        Holder.UpdateToken(token);
    }

    public void SetModificationDateTime(DateTime modifiedAt)
    {
        _modifiedAt = modifiedAt;
    }

    public void SetCreationDateTime(DateTime createdAt)
    {
        _createdAt = createdAt;
    }

    public Money ConvertToUSD(Money money, Currency currency)
    {
        return money / currency.DollarExchangeRate;
    }

    public Money ConvertFromUSD(Money money)
    {
        return money * DisplayCurrency.DollarExchangeRate;
    }

    private void AddBalanceChangedDomainEvent(DateTime transactionDateTime)
    {
        AddDomainEvent(new AccountBalanceChangedDomainEvent(BalanceInUSD, transactionDateTime));
    }

    private Transaction SendTransfer(Money amount, Account receiver, DateTime transactionDateTime)
    {
        Debit(amount);

        var transaction = new Transaction(amount, BalanceInUSD, TransactionType.TransferOut, Id,  receiver.Id, transactionDateTime);

        _transactions.Add(transaction);

        return transaction;
    }

    private void ReceiveTransfer(Money amount, Account sender, DateTime transactionDateTime)
    {
        Credit(amount);

        var transaction = new Transaction(amount, BalanceInUSD, TransactionType.TransferIn, sender.Id,  Id, transactionDateTime);

        _transactions.Add(transaction);
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
        var usd = ConvertToUSD(amount, DisplayCurrency);

        BalanceInUSD = new Money(BalanceInUSD + usd);

        return usd;
    }

    private static DateTime StartOfDay(DateTime dateTime) =>
        new(dateTime.Year, dateTime.Month, dateTime.Day, 0, 0, 0, 0);

    private static DateTime EndOfDay(DateTime dateTime) =>
        new(dateTime.Year, dateTime.Month, dateTime.Day, 23, 59, 59, 999);
}