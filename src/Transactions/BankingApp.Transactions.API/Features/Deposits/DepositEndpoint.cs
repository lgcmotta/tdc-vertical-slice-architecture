using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace BankingApp.Transactions.API.Features.Deposits;

public static class DepositEndpoint
{
    public static async Task<IResult> PostAsync(
        [FromServices] IMediator mediator,
        [FromBody] DepositRequest request,
        CancellationToken cancellationToken = default)
    {
        var command = new DepositCommand(request.Token, request.Currency, request.Amount);

        var response = await mediator.Send(command, cancellationToken)
            .ConfigureAwait(continueOnCapturedContext: false);

        return Results.Ok(response);
    }
}