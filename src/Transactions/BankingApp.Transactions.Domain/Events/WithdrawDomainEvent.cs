using MediatR;

namespace BankingApp.Transactions.Domain.Events;

public record WithdrawDomainEvent(Guid HolderId, decimal Balance) : INotification;