namespace BankingApp.Taxes.Domain.Events;

public record OverdraftFeeCalculatedDomainEvent(Guid HolderId, decimal FeeAmount);