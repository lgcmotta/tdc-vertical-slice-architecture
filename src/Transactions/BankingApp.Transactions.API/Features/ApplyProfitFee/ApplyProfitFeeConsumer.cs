using BankingApp.Messaging.Contracts;
using MassTransit;
using MediatR;

namespace BankingApp.Transactions.API.Features.ApplyProfitFee;

public class ApplyProfitFeeConsumer : IConsumer<AccountProfitFeeSettledIntegrationEvent>
{
    private readonly IMediator _mediator;

    public ApplyProfitFeeConsumer(IMediator mediator)
    {
        _mediator = mediator;
    }

    public async Task Consume(ConsumeContext<AccountProfitFeeSettledIntegrationEvent> context)
    {
        var message = context.Message;

        var command = new ApplyProfitFeeCommand(message.HolderId, message.ProfitFee);

        await _mediator.Send(command, context.CancellationToken)
            .ConfigureAwait(continueOnCapturedContext: false);
    }
}