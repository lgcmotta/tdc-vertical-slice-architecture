using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace BankingApp.Transactions.API.Features.MonthlyStatement;

public static class MonthlyStatementEndpoint
{
    public static async Task<IResult> GetAsync(
        [FromServices] IMediator mediator,
        [FromRoute] string token,
        [FromRoute] int year,
        [FromRoute] int month,
        CancellationToken cancellationToken = default)
    {
        var query = new MonthlyStatementQuery(token, year, month);

        var response = await mediator.Send(query, cancellationToken).ConfigureAwait(continueOnCapturedContext: false);

        return Results.Ok(response);
    }
}