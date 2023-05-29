using MediatR;

namespace BankingApp.Accounts.Domain.Events;

public record AccountUpdatedDomainEvent(Guid HolderId, string Name, string Token, string Currency) : INotification;