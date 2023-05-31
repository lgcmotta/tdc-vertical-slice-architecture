using BankingApp.Messaging.Contracts;
using BankingApp.Transactions.Domain.Events;
using MassTransit;
using MediatR;

namespace BankingApp.Transactions.API.Features.Deposit;

public class DepositDomainEventHandler : INotificationHandler<DepositDomainEvent>
{
    private readonly IPublishEndpoint _publishEndpoint;

    public DepositDomainEventHandler(IPublishEndpoint publishEndpoint)
    {
        _publishEndpoint = publishEndpoint;
    }

    public async Task Handle(DepositDomainEvent notification, CancellationToken cancellationToken)
    {
        var integrationEvent = new AccountBalanceChangedIntegrationEvent(notification.HolderId, notification.Balance);

        await _publishEndpoint.Publish(integrationEvent, cancellationToken)
            .ConfigureAwait(continueOnCapturedContext: false);
    }
}