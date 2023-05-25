﻿using MediatR;
using System.Collections.ObjectModel;

namespace BankingApp.Domain.Core;

public interface IAggregateRoot
{
    public IReadOnlyCollection<INotification> DomainEvents { get; }

    public void AddDomainEvent(INotification domainEvent);

    public void RemoveDomainEvent(INotification domainEvent);

    public void ClearDomainEvents();
}

public abstract class AggregateRoot<TId> : IEntity<TId>, IAggregateRoot
{
    private readonly List<INotification> _domainEvents = new();

    public TId Id { get; protected set;  }

    public IReadOnlyCollection<INotification> DomainEvents => new ReadOnlyCollection<INotification>(_domainEvents);

    public void AddDomainEvent(INotification domainEvent) => _domainEvents.Add(domainEvent);

    public void RemoveDomainEvent(INotification domainEvent) => _domainEvents.Remove(domainEvent);

    public void ClearDomainEvents() => _domainEvents.Clear();
}