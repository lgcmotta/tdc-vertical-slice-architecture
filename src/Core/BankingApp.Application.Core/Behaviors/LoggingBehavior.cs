using BankingApp.Infrastructure.Core.Extensions;
using MediatR;
using Microsoft.Extensions.Logging;

namespace BankingApp.Application.Core.Behaviors;

public class LoggingBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : notnull
{
    private readonly ILogger _logger;

    public LoggingBehavior(ILogger logger)
    {
        _logger = logger;
    }

    public LoggingBehavior(ILogger<LoggingBehavior<TRequest, TResponse>> logger)
    {
        _logger = logger;
    }

    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        var behaviorName = typeof(LoggingBehavior<TRequest, TResponse>).GetGenericTypeName();

        var requestType = typeof(TRequest);

        try
        {
            _logger.LogInformation("[{Behavior}] - Handling request of type {RequestType}", behaviorName, requestType);

            var response = await next().ConfigureAwait(continueOnCapturedContext: false);

            _logger.LogInformation("[{Behavior}] - Request of type {RequestType} handled successfully", behaviorName, requestType);

            return response;
        }
        catch (Exception exception)
        {
            _logger.LogError(exception, "[{Behavior}] - An exception occurred while handling request of type {RequestType}", behaviorName, requestType);

            throw;
        }
    }
}