using BankingApp.Messaging.Contracts;
using BankingApp.Transactions.Domain.Events;
using MassTransit;
using MediatR;

namespace BankingApp.Transactions.API.Features.Transfer;

public class TransferOutDomainEventHandler : INotificationHandler<TransferOutDomainEvent>
{
    private readonly IPublishEndpoint _publishEndpoint;

    public TransferOutDomainEventHandler(IPublishEndpoint publishEndpoint)
    {
        _publishEndpoint = publishEndpoint;
    }

    public async Task Handle(TransferOutDomainEvent notification, CancellationToken cancellationToken)
    {
        var integrationEvent = new AccountBalanceChangedIntegrationEvent(notification.HolderId, notification.Balance);

        await _publishEndpoint.Publish(integrationEvent, cancellationToken)
            .ConfigureAwait(continueOnCapturedContext: false);
    }
}