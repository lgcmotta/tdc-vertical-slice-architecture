using MediatR;

namespace BankingApp.Accounts.Domain.Events;

public record AccountTokenChangedDomainEvent(Guid HolderId, string Token) : INotification;