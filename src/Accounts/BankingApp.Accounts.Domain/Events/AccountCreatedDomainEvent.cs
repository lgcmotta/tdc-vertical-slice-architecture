using MediatR;

namespace BankingApp.Accounts.Domain.Events;

public record AccountCreatedDomainEvent(Guid HolderId, string Name, string Document, string Token, string Currency) : INotification;