using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace BankingApp.Transactions.API.Features.Deposits;

public static class CreateDepositEndpoint
{
    public static async Task<IResult> PostAsync(
        [FromServices] IMediator mediator,
        [FromBody] CreateDepositRequest request,
        CancellationToken cancellationToken = default)
    {
        var command = new CreateDepositCommand(request.Token, request.Currency, request.Amount);

        var response = await mediator.Send(command, cancellationToken)
            .ConfigureAwait(continueOnCapturedContext: false);

        return Results.Ok(response);
    }
}