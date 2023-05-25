using MediatR;

namespace BankingApp.Domain.Core;

public interface IAggregateRoot
{
    public IReadOnlyCollection<INotification> DomainEvents { get; }

    public void AddDomainEvent(INotification domainEvent);

    public void RemoveDomainEvent(INotification domainEvent);

    public void ClearDomainEvents();
}