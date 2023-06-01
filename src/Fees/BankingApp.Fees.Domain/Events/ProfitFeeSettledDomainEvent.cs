using MediatR;

namespace BankingApp.Fees.Domain.Events;

public record ProfitFeeSettledDomainEvent(Guid HolderId, decimal FeeAmount) : INotification;