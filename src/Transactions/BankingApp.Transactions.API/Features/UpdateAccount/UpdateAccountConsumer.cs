﻿using BankingApp.Messaging.Contracts;
using MassTransit;
using MediatR;

namespace BankingApp.Transactions.API.Features.UpdateAccount;

public class UpdateAccountConsumer : IConsumer<AccountUpdatedIntegrationEvent>
{
    private readonly IMediator _mediator;

    public UpdateAccountConsumer(IMediator mediator)
    {
        _mediator = mediator;
    }

    public async Task Consume(ConsumeContext<AccountUpdatedIntegrationEvent> context)
    {
        var message = context.Message;

        var command = new UpdateAccountCommand(
            message.HolderId,
            message.Name,
            message.Token,
            message.Currency
        );

        await _mediator.Send(command, context.CancellationToken)
            .ConfigureAwait(continueOnCapturedContext: false);
    }
}