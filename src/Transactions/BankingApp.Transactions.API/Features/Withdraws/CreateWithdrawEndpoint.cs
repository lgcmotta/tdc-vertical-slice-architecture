using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace BankingApp.Transactions.API.Features.Withdraws;

public static class CreateWithdrawEndpoint
{
    public static async Task<IResult> PostAsync(
        [FromServices] IMediator mediator,
        [FromBody] CreateWithdrawCommand request,
        CancellationToken cancellationToken = default)
    {
        var command = new CreateWithdrawCommand(request.Token, request.Amount);

        var response = await mediator.Send(command, cancellationToken)
            .ConfigureAwait(continueOnCapturedContext: false);

        return Results.Ok(response);
    }
}