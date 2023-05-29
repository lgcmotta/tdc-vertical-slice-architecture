using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace BankingApp.Accounts.API.Features.AccountDetails;

public static class AccountDetailsEndpoint
{
    public static async Task<IResult> GetAsync(
        [FromServices] IMediator mediator,
        [FromRoute] string token,
        CancellationToken cancellationToken = default)
    {
        var query = new AccountDetailsQuery(token);

        var response = await mediator.Send(query, cancellationToken).ConfigureAwait(continueOnCapturedContext: false);

        return Results.Ok(response);
    }
}