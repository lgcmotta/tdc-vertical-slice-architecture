using BankingApp.Accounts.Domain.Events;
using BankingApp.Messaging.Contracts;
using MassTransit;
using MediatR;

namespace BankingApp.Accounts.API.Features.UpdateAccount;

public class AccountUpdatedDomainEventHandler : INotificationHandler<AccountUpdatedDomainEvent>
{
    private readonly IPublishEndpoint _publishEndpoint;

    public AccountUpdatedDomainEventHandler(IPublishEndpoint publishEndpoint)
    {
        _publishEndpoint = publishEndpoint;
    }

    public async Task Handle(AccountUpdatedDomainEvent notification, CancellationToken cancellationToken)
    {
        var integrationEvent = new AccountUpdatedIntegrationEvent(
            notification.HolderId,
            notification.Name,
            notification.Token,
            notification.Currency
        );

        await _publishEndpoint.Publish(integrationEvent, cancellationToken)
            .ConfigureAwait(continueOnCapturedContext: false);
    }
}