using BankingApp.Messaging.Contracts;
using MassTransit;
using MediatR;

namespace BankingApp.Transactions.API.Features.ApplyProfitFee;

public class ApplyProfitFeeConsumer : IConsumer<AccountEarningsIntegrationEvent>
{
    private readonly IMediator _mediator;

    public ApplyProfitFeeConsumer(IMediator mediator)
    {
        _mediator = mediator;
    }

    public async Task Consume(ConsumeContext<AccountEarningsIntegrationEvent> context)
    {
        var message = context.Message;

        var command = new ApplyProfitFeeCommand(message.HolderId, message.Earnings);

        await _mediator.Send(command, context.CancellationToken)
            .ConfigureAwait(continueOnCapturedContext: false);
    }
}