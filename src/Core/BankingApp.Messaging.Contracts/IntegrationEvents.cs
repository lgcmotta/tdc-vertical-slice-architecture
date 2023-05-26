using MassTransit;

// ReSharper disable ClassNeverInstantiated.Global
namespace BankingApp.Messaging.Contracts;

[MessageUrn("account-created")]
public record AccountCreatedIntegrationEvent(Guid HolderId, string Name, string Document, string Token, string Currency);

[MessageUrn("account-updated")]
public record AccountUpdatedIntegrationEvent(Guid HolderId, string? Name, string? Document, string? Token, string? Currency);

[MessageUrn("account-balance-changed")]
public record AccountBalanceChangedIntegrationEvent(Guid HolderId, decimal Balance, DateTime TransactionTimeStamp);