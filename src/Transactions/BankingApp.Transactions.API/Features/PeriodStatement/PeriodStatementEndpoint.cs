using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace BankingApp.Transactions.API.Features.PeriodStatement;

public static class PeriodStatementEndpoint
{
    public static async Task<IResult> GetAsync(
        [FromServices] IMediator mediator,
        [FromRoute] string token,
        [FromQuery] DateOnly? start = null,
        [FromQuery] DateOnly? end = null,
        CancellationToken cancellationToken = default)
    {

        var query = new PeriodStatementQuery(token, start, end);

        var response = await mediator.Send(query, cancellationToken)
            .ConfigureAwait(continueOnCapturedContext: false);

        return response.Any() ? Results.Ok(response) : Results.NotFound();
    }
}