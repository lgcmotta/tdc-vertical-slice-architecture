using BankingApp.Messaging.Contracts;
using MassTransit;
using MediatR;

namespace BankingApp.Transactions.API.Features.ApplyOverdraftFee;

public class ApplyOverdraftFeeConsumer : IConsumer<AccountOverdraftSettledIntegrationEvent>
{
    private readonly IMediator _mediator;

    public ApplyOverdraftFeeConsumer(IMediator mediator)
    {
        _mediator = mediator;
    }

    public async Task Consume(ConsumeContext<AccountOverdraftSettledIntegrationEvent> context)
    {
        var message = context.Message;

        var command = new ApplyOverdraftFeeCommand(message.HolderId, message.OverdraftFee);

        await _mediator.Send(command, context.CancellationToken)
            .ConfigureAwait(continueOnCapturedContext: false);
    }
}