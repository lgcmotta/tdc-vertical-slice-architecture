using MediatR;

namespace BankingApp.Taxes.Domain.Events;

public record OverdraftFeeSettledDomainEvent(Guid HolderId, decimal FeeAmount) : INotification;