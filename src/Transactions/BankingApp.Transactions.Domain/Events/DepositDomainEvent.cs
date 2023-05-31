using MediatR;

namespace BankingApp.Transactions.Domain.Events;

public record DepositDomainEvent(Guid HolderId, decimal Balance) : INotification;