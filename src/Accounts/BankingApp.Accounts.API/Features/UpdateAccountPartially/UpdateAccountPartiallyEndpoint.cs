using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace BankingApp.Accounts.API.Features.UpdateAccountPartially;

public static class UpdateAccountPartiallyEndpoint
{
    public static async Task<IResult> PatchAsync(
        [FromServices] IMediator mediator,
        [FromRoute] string token,
        [FromBody] UpdateAccountPartiallyRequest request,
        CancellationToken cancellationToken = default)
    {
        if (token != request.Token)
        {
            return Results.BadRequest(new { Error = "The provided token does not match the sender token in the request body." });
        }

        var command = new UpdateAccountPartiallyCommand(
            request.Token,
            request.FirstName,
            request.LastName,
            request.Document,
            request.Currency
        );

        await mediator.Send(command, cancellationToken).ConfigureAwait(continueOnCapturedContext: false);

        return Results.NoContent();
    }
}