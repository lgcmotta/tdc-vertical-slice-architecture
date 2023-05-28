using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace BankingApp.Transactions.API.Features.Transfers;

public static class TransferEndpoint
{
    public static async Task<IResult> PostAsync(
        [FromServices] IMediator mediator,
        [FromRoute] string token,
        [FromBody] TransferRequest request,
        CancellationToken cancellationToken = default)
    {
        if (token != request.SenderToken)
        {
            return Results.BadRequest(new { Error = "The provided sender token does not match the sender token in the request body." });
        }

        var command = new TransferCommand(request.Amount, request.Currency, request.SenderToken, request.ReceiverToken);

        var response = await mediator.Send(command, cancellationToken);

        return Results.Ok(response);
    }
}