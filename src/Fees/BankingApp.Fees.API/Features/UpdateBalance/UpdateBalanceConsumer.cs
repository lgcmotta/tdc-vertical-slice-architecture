using BankingApp.Messaging.Contracts;
using MassTransit;
using MediatR;

namespace BankingApp.Fees.API.Features.UpdateBalance;

public class UpdateBalanceConsumer : IConsumer<AccountBalanceChangedIntegrationEvent>
{
    private readonly IMediator _mediator;

    public UpdateBalanceConsumer(IMediator mediator)
    {
        _mediator = mediator;
    }

    public async Task Consume(ConsumeContext<AccountBalanceChangedIntegrationEvent> context)
    {
        var message = context.Message;

        var command = new UpdateBalanceCommand(message.HolderId, message.Balance);

        await _mediator.Send(command, context.CancellationToken).ConfigureAwait(continueOnCapturedContext: false);
    }
}