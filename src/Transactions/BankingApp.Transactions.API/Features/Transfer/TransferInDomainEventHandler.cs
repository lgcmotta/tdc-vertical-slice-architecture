﻿using BankingApp.Messaging.Contracts;
using BankingApp.Transactions.Domain.Events;
using MassTransit;
using MediatR;

namespace BankingApp.Transactions.API.Features.Transfer;

public class TransferInDomainEventHandler : INotificationHandler<TransferInDomainEvent>
{
    private readonly IPublishEndpoint _publishEndpoint;

    public TransferInDomainEventHandler(IPublishEndpoint publishEndpoint)
    {
        _publishEndpoint = publishEndpoint;
    }

    public async Task Handle(TransferInDomainEvent notification, CancellationToken cancellationToken)
    {
        var (holderId, balance) = notification;

        var integrationEvent = new AccountBalanceChangedIntegrationEvent(holderId, balance);

        await _publishEndpoint.Publish(integrationEvent, cancellationToken)
            .ConfigureAwait(continueOnCapturedContext: false);;
    }
}