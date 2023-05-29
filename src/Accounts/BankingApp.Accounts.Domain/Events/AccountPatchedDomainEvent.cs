using MediatR;

namespace BankingApp.Accounts.Domain.Events;

public record AccountPatchedDomainEvent(Guid HolderId, string? Name, string? Token, string? Currency) : INotification;