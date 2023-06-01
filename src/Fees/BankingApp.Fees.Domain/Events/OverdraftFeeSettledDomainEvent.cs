using MediatR;

namespace BankingApp.Fees.Domain.Events;

public record OverdraftFeeSettledDomainEvent(Guid HolderId, decimal FeeAmount) : INotification;