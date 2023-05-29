using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace BankingApp.Transactions.API.Features.RetrievePeriodStatement;

public static class RetrievePeriodStatementEndpoint
{
    public static async Task<IResult> GetAsync(
        [FromServices] IMediator mediator,
        [FromRoute] string token,
        [FromQuery] DateOnly? start = null,
        [FromQuery] DateOnly? end = null,
        CancellationToken cancellationToken = default)
    {
        var query = new RetrievePeriodStatementQuery(token, start, end);

        var response = await mediator.Send(query, cancellationToken)
            .ConfigureAwait(continueOnCapturedContext: false);

        return Results.Ok(response);
    }
}