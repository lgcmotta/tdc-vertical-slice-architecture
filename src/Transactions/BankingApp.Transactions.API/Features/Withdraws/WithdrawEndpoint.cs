using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace BankingApp.Transactions.API.Features.Withdraws;

public static class WithdrawEndpoint
{
    public static async Task<IResult> PostAsync(
        [FromServices] IMediator mediator,
        [FromBody] WithdrawCommand request,
        CancellationToken cancellationToken = default)
    {
        var command = new WithdrawCommand(request.Token, request.Amount);

        var response = await mediator.Send(command, cancellationToken)
            .ConfigureAwait(continueOnCapturedContext: false);

        return Results.Ok(response);
    }
}