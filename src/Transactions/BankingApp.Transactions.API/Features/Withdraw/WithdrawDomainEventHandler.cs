using BankingApp.Messaging.Contracts;
using BankingApp.Transactions.Domain.Events;
using MassTransit;
using MediatR;

namespace BankingApp.Transactions.API.Features.Withdraw;

public class WithdrawDomainEventHandler : INotificationHandler<WithdrawDomainEvent>
{
    private readonly IPublishEndpoint _publishEndpoint;

    public WithdrawDomainEventHandler(IPublishEndpoint publishEndpoint)
    {
        _publishEndpoint = publishEndpoint;
    }

    public async Task Handle(WithdrawDomainEvent notification, CancellationToken cancellationToken)
    {
        var integrationEvent = new AccountBalanceChangedIntegrationEvent(notification.HolderId, notification.Balance);

        await _publishEndpoint.Publish(integrationEvent, cancellationToken)
            .ConfigureAwait(continueOnCapturedContext: false);
    }
}