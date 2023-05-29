using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace BankingApp.Transactions.API.Features.RetrieveYearlyStatement;

public static class RetrieveYearlyStatementEndpoint
{
    public static async Task<IResult> GetAsync(
        [FromServices] IMediator mediator,
        [FromRoute] string token,
        [FromRoute] int year,
        CancellationToken cancellationToken = default)
    {
        var query = new RetrieveYearlyStatementQuery(token, year);

        var response = await mediator.Send(query, cancellationToken).ConfigureAwait(continueOnCapturedContext: false);

        return Results.Ok(response);
    }
}