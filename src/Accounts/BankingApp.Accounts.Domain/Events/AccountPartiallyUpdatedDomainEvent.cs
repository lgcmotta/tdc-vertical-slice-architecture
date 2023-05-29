using MediatR;

namespace BankingApp.Accounts.Domain.Events;

public record AccountPartiallyUpdatedDomainEvent(Guid HolderId, string? Name, string? Token, string? Currency) : INotification;