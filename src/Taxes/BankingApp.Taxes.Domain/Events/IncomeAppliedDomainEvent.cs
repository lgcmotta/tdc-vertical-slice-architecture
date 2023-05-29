namespace BankingApp.Taxes.Domain.Events;

public record IncomeAppliedDomainEvent(Guid HolderId, decimal IncomeAmount);