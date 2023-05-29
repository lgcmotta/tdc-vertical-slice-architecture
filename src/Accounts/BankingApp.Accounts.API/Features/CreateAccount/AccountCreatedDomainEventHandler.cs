using BankingApp.Accounts.Domain.Events;
using BankingApp.Messaging.Contracts;
using MassTransit;
using MediatR;

namespace BankingApp.Accounts.API.Features.CreateAccount;

public class AccountCreatedDomainEventHandler : INotificationHandler<AccountCreatedDomainEvent>
{
    private readonly IPublishEndpoint _publishEndpoint;

    public AccountCreatedDomainEventHandler(IPublishEndpoint publishEndpoint)
    {
        _publishEndpoint = publishEndpoint;
    }

    public async Task Handle(AccountCreatedDomainEvent notification, CancellationToken cancellationToken)
    {
        var integrationEvent = new AccountCreatedIntegrationEvent(
            notification.HolderId,
            notification.Name,
            notification.Document,
            notification.Token,
            notification.Currency
        );

        await _publishEndpoint.Publish(integrationEvent, cancellationToken)
            .ConfigureAwait(continueOnCapturedContext: false);
    }
}