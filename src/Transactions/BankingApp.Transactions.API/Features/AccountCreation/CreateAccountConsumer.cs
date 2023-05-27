using BankingApp.Messaging.Contracts;
using MassTransit;
using MediatR;

namespace BankingApp.Transactions.API.Features.AccountCreation;

public class CreateAccountConsumer : IConsumer<AccountCreatedIntegrationEvent>
{
    private readonly IMediator _mediator;

    public CreateAccountConsumer(IMediator mediator)
    {
        _mediator = mediator;
    }

    public async Task Consume(ConsumeContext<AccountCreatedIntegrationEvent> context)
    {
        var message = context.Message;

        var command = new CreateAccountCommand(
            message.HolderId,
            message.Name,
            message.Document,
            message.Token,
            message.Currency
        );

        await _mediator.Send(command, context.CancellationToken)
            .ConfigureAwait(continueOnCapturedContext: false);
    }
}