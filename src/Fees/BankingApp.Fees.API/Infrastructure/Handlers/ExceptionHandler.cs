using BankingApp.Infrastructure.Core.Handlers;

namespace BankingApp.Fees.API.Infrastructure.Handlers;

public class ExceptionHandler : IExceptionHandler
{
    public async Task HandleAsync(HttpContext context, Exception exception)
    {
        throw new NotImplementedException();
    }
}