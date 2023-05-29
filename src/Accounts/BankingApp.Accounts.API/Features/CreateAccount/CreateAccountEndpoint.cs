using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace BankingApp.Accounts.API.Features.CreateAccount;

public static class CreateAccountEndpoint
{
    public static async Task<IResult> PostAsync(
        [FromServices] IMediator mediator,
        [FromBody] CreateAccountRequest request,
        CancellationToken cancellationToken = default)
    {
        var command = new CreateAccountCommand(request.FirstName, request.LastName, request.Document, request.Currency);

        var response = await mediator.Send(command, cancellationToken).ConfigureAwait(continueOnCapturedContext: false);

        return Results.Ok(response);
    }
}