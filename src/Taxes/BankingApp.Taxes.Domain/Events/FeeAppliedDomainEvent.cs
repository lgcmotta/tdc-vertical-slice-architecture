namespace BankingApp.Taxes.Domain.Events;

public record FeeAppliedDomainEvent(Guid HolderId, decimal FeeAmount);