using System;
using System.Collections.Generic;
using MediatR;

namespace DesafioWarren.Domain.Entities
{
    public class Entity
    {
        private readonly List<INotification> _domainEvents = new();
        
        public Guid Id { get; protected set; }

        public IReadOnlyCollection<INotification> DomainEvents => _domainEvents;

        public void AddDomainEvent(INotification @event) => _domainEvents.Add(@event);

        public void RemoveDomainEvent(INotification @event) => _domainEvents.Remove(@event);

        public void ClearDomainEvents() => _domainEvents.Clear();
    }
}