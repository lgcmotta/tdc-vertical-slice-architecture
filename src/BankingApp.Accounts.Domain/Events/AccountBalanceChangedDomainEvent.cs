using MediatR;

namespace BankingApp.Accounts.Domain.Events;

public class AccountBalanceChangedDomainEvent : INotification
{
    public decimal Balance { get; private set; }

    public DateTime TransactionDateTime { get; private set; }

    public AccountBalanceChangedDomainEvent(decimal balance, DateTime transactionDateTime)
    {
        Balance = balance;
        TransactionDateTime = transactionDateTime;
    }
}