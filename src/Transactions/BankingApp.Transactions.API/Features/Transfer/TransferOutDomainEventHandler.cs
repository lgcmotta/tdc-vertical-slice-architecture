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
        var (holderId, balance) = notification;

        var integrationEvent = new AccountBalanceChangedIntegrationEvent(holderId, balance);

        await _publishEndpoint.Publish(integrationEvent, cancellationToken)
            .ConfigureAwait(continueOnCapturedContext: false);
    }
}