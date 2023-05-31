using MediatR;

namespace BankingApp.Transactions.Domain.Events;

public record TransferOutDomainEvent(Guid HolderId, decimal Balance) : INotification;