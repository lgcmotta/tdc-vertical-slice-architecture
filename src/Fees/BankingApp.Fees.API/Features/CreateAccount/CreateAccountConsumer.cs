using BankingApp.Messaging.Contracts;
using MassTransit;
using MediatR;

namespace BankingApp.Fees.API.Features.CreateAccount;

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

        var command = new CreateAccountCommand(message.HolderId, message.Token);

        await _mediator.Send(command, context.CancellationToken).ConfigureAwait(continueOnCapturedContext: false);
    }
}