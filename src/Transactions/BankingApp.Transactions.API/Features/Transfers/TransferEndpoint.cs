using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace BankingApp.Transactions.API.Features.Transfers;

public static class TransferEndpoint
{
    public static async Task<IResult> PostAsync(
        [FromServices] IMediator mediator,
        [FromBody] TransferRequest request,
        CancellationToken cancellationToken = default)
    {
        var command = new TransferCommand(request.Amount, request.SenderToken, request.ReceiverToken);

        var response = await mediator.Send(command, cancellationToken);

        return Results.Ok(response);
    }
}