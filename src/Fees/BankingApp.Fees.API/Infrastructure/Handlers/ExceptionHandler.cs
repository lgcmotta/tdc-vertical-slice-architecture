﻿using BankingApp.Application.Core.Exceptions;
using BankingApp.Fees.Domain.Exceptions;
using BankingApp.Infrastructure.Core.Handlers;
using System.Net;

namespace BankingApp.Fees.API.Infrastructure.Handlers;

public class ExceptionHandler : IExceptionHandler
{
    public async Task HandleAsync(HttpContext context, Exception exception)
    {
        switch (exception)
        {
            case AccountNotFoundException
                or AccountHolderConflictException:
                context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                await context.Response.WriteAsJsonAsync(new { Error = exception.Message });
                return;
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