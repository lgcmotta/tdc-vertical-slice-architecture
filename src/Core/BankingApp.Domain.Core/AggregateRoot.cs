using MediatR;

namespace BankingApp.Domain.Core;

public abstract class AggregateRoot<TId> : IEntity<TId>, IAggregateRoot
{
    private readonly List<INotification> _domainEvents = new();

    public TId Id { get; protected set; } = default!;

    public IReadOnlyCollection<INotification> DomainEvents => _domainEvents.AsReadOnly();

    public void AddDomainEvent(INotification domainEvent) => _domainEvents.Add(domainEvent);

    public void RemoveDomainEvent(INotification domainEvent) => _domainEvents.Remove(domainEvent);

    public void ClearDomainEvents() => _domainEvents.Clear();
}