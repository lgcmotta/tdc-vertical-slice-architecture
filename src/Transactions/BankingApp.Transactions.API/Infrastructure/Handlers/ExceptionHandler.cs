using BankingApp.Application.Core.Exceptions;
using BankingApp.Infrastructure.Core.Handlers;
using System.Net;

namespace BankingApp.Transactions.API.Infrastructure.Handlers;

public class ExceptionHandler : IExceptionHandler
{
    public async Task HandleAsync(HttpContext context, Exception exception)
    {
        if (exception is ValidationFailedException validationFailedException)
        {
            context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
            await context.Response.WriteAsJsonAsync(validationFailedException.Errors);
            return;
        }
    }
}