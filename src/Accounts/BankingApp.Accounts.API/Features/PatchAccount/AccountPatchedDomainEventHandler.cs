using BankingApp.Accounts.Domain.Events;
using BankingApp.Messaging.Contracts;
using MassTransit;
using MediatR;

namespace BankingApp.Accounts.API.Features.PatchAccount;

public class AccountPatchedDomainEventHandler : INotificationHandler<AccountPatchedDomainEvent>
{
    private readonly IPublishEndpoint _publishEndpoint;

    public AccountPatchedDomainEventHandler(IPublishEndpoint publishEndpoint)
    {
        _publishEndpoint = publishEndpoint;
    }

    public async Task Handle(AccountPatchedDomainEvent notification, CancellationToken cancellationToken)
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