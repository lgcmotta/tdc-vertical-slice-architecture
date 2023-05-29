using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace BankingApp.Accounts.API.Features.RetrieveAccountDetails;

public static class RetrieveAccountDetailsEndpoint
{
    public static async Task<IResult> GetAsync(
        [FromServices] IMediator mediator,
        [FromRoute] string token,
        CancellationToken cancellationToken = default)
    {
        var query = new RetrieveAccountDetailsQuery(token);

        var response = await mediator.Send(query, cancellationToken).ConfigureAwait(continueOnCapturedContext: false);

        return Results.Ok(response);
    }
}