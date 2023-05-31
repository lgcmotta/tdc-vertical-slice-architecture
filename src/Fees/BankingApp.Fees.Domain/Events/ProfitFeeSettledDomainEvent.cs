namespace BankingApp.Taxes.Domain.Events;

public record ProfitFeeSettledDomainEvent(Guid HolderId, decimal FeeAmount);