using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace BankingApp.Transactions.API.Features.Deposit;

public static class DepositEndpoint
{
    public static async Task<IResult> PostAsync(
        [FromServices] IMediator mediator,
        [FromRoute] string token,
        [FromBody] DepositRequest request,
        CancellationToken cancellationToken = default)
    {
        if (token != request.Token)
        {
            return Results.BadRequest(new { Error = "The provided token does not match the sender token in the request body." });
        }

        var command = new DepositCommand(request.Token, request.Currency, request.Amount);

        var response = await mediator.Send(command, cancellationToken)
            .ConfigureAwait(continueOnCapturedContext: false);

        return Results.Ok(response);
    }
}