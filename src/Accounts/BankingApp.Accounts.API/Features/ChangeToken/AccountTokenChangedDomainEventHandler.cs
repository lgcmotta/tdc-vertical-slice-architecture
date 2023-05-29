using BankingApp.Accounts.Domain.Events;
using BankingApp.Messaging.Contracts;
using MassTransit;
using MediatR;

namespace BankingApp.Accounts.API.Features.ChangeToken;

public class AccountTokenChangedDomainEventHandler : INotificationHandler<AccountTokenChangedDomainEvent>
{
    private readonly IPublishEndpoint _publishEndpoint;

    public AccountTokenChangedDomainEventHandler(IPublishEndpoint publishEndpoint)
    {
        _publishEndpoint = publishEndpoint;
    }

    public async Task Handle(AccountTokenChangedDomainEvent notification, CancellationToken cancellationToken)
    {
        var integrationEvent = new AccountUpdatedIntegrationEvent(
            notification.HolderId,
            Name: null,
            notification.Token,
            Currency: null
        );

        await _publishEndpoint.Publish(integrationEvent, cancellationToken)
            .ConfigureAwait(continueOnCapturedContext: false);
    }
}