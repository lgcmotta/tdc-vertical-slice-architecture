using BankingApp.Fees.Domain.Events;
using BankingApp.Messaging.Contracts;
using MassTransit;
using MediatR;

namespace BankingApp.Fees.API.Features.ProfitFee;

public class ProfitFeeSettledDomainEventHandler : INotificationHandler<ProfitFeeSettledDomainEvent>
{
    private readonly IPublishEndpoint _publishEndpoint;

    public ProfitFeeSettledDomainEventHandler(IPublishEndpoint publishEndpoint)
    {
        _publishEndpoint = publishEndpoint;
    }

    public async Task Handle(ProfitFeeSettledDomainEvent notification, CancellationToken cancellationToken)
    {
        var integrationEvent = new AccountProfitFeeSettledIntegrationEvent(notification.HolderId, notification.FeeAmount);

        await _publishEndpoint.Publish(integrationEvent, cancellationToken)
            .ConfigureAwait(continueOnCapturedContext: false);
    }
}