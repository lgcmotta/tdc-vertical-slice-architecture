using BankingApp.Messaging.Contracts;
using BankingApp.Taxes.Domain.Events;
using MassTransit;
using MediatR;

namespace BankingApp.Fees.API.Features.OverdraftFee;

public class OverdraftFeeSettledDomainEventHandler : INotificationHandler<OverdraftFeeSettledDomainEvent>
{
    private readonly IPublishEndpoint _publishEndpoint;

    public OverdraftFeeSettledDomainEventHandler(IPublishEndpoint publishEndpoint)
    {
        _publishEndpoint = publishEndpoint;
    }

    public async Task Handle(OverdraftFeeSettledDomainEvent notification, CancellationToken cancellationToken)
    {
        var integrationEvent = new AccountOverdraftSettledIntegrationEvent(notification.HolderId, notification.FeeAmount);

        await _publishEndpoint.Publish(integrationEvent, cancellationToken)
            .ConfigureAwait(continueOnCapturedContext: false);
    }
}