using BankingApp.Messaging.Contracts;
using MassTransit;
using MediatR;

namespace BankingApp.Transactions.API.Features.Earnings;

public class ApplyEarningsConsumer : IConsumer<AccountEarningsIntegrationEvent>
{
    private readonly IMediator _mediator;

    public ApplyEarningsConsumer(IMediator mediator)
    {
        _mediator = mediator;
    }

    public async Task Consume(ConsumeContext<AccountEarningsIntegrationEvent> context)
    {
        var message = context.Message;

        var command = new ApplyEarningsCommand(message.HolderId, message.Earnings);

        await _mediator.Send(command, context.CancellationToken)
            .ConfigureAwait(continueOnCapturedContext: false);
    }
}