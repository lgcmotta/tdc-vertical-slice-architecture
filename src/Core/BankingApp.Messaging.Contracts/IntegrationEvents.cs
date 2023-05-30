// ReSharper disable ClassNeverInstantiated.Global
namespace BankingApp.Messaging.Contracts;

public record AccountCreatedIntegrationEvent(Guid HolderId, string Name, string Document, string Token, string Currency);

public record AccountUpdatedIntegrationEvent(Guid HolderId, string? Name, string? Token, string? Currency);

public record AccountBalanceChangedIntegrationEvent(Guid HolderId, decimal Balance, DateTime TransactionTimeStamp);

public record AccountEarningsIntegrationEvent(Guid HolderId, decimal Earnings);