using MediatR;

namespace BankingApp.Transactions.Domain.Events;

public record TransferInDomainEvent(Guid HolderId, decimal Balance) : INotification;