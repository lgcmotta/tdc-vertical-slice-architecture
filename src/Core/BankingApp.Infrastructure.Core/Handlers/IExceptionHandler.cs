using Microsoft.AspNetCore.Http;

namespace BankingApp.Infrastructure.Core.Handlers;

public interface IExceptionHandler
{
    Task HandleAsync(HttpContext context, Exception exception);
}