using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace BankingApp.Accounts.API.Features.ChangeToken;

public static class ChangeTokenEndpoint
{
    public static async Task<IResult> PostAsync(
        [FromServices] IMediator mediator,
        [FromRoute] string token,
        CancellationToken cancellationToken = default)
    {
        var command = new ChangeTokenCommand(token);

        var response = await mediator.Send(command, cancellationToken).ConfigureAwait(continueOnCapturedContext: false);

        return Results.Ok(response);
    }
}