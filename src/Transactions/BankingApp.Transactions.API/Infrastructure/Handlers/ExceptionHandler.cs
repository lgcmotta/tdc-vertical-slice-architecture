using BankingApp.Application.Core.Exceptions;
using BankingApp.Infrastructure.Core.Handlers;
using BankingApp.Transactions.Domain.Exceptions;
using System.Net;

namespace BankingApp.Transactions.API.Infrastructure.Handlers;

public class ExceptionHandler : IExceptionHandler
{
    public async Task HandleAsync(HttpContext context, Exception exception)
    {
        switch (exception)
        {
            case InvalidTransactionValueException _
                or AccountNotFoundException _
                or AccountHolderConflictException _:
                context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                await context.Response.WriteAsJsonAsync(new { Error = exception.Message });
                break;
            case ValidationFailedException validationFailedException:
                context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                await context.Response.WriteAsJsonAsync(validationFailedException.Errors);
                return;
            default:
                context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                await context.Response.WriteAsJsonAsync(new { Error = "Unexpected error. Please contact the support." });
                return;
        }
    }
}