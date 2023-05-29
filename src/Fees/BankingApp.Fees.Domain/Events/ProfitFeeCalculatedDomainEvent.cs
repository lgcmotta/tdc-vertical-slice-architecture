namespace BankingApp.Taxes.Domain.Events;

public record ProfitFeeCalculatedDomainEvent(Guid HolderId, decimal IncomeAmount);