using DesafioWarren.Domain.Aggregates;
using MediatR;

namespace DesafioWarren.Domain.DomainEvents
{
    public class AccountBalanceChangedDomainEvent : INotification
    {
        public Account Account { get; private set; }

        public AccountBalanceChangedDomainEvent(Account account)
        {
            Account = account;
        }
    }
}