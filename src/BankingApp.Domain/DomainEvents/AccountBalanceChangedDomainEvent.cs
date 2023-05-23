using BankingApp.Domain.Aggregates;
using MediatR;

namespace BankingApp.Domain.DomainEvents;

public class AccountBalanceChangedDomainEvent : INotification
{
    public Account Account { get; private set; }

    public AccountBalanceChangedDomainEvent(Account account)
    {
        Account = account;
    }
}