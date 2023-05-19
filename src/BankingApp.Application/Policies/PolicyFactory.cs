using Polly;
using Polly.Retry;
using Serilog;
using System;

namespace BankingApp.Application.Policies;

public class PolicyFactory
{
    public static AsyncRetryPolicy CreateAsyncRetryPolicy(ILogger logger) => Policy.Handle<Exception>()
        .WaitAndRetryForeverAsync(sleepDurationProvider => TimeSpan.FromSeconds(5), (exception, attempt, context) =>
        {
            logger.Error(exception, "----------------- An exception of type {ExceptionType} with message {Message} has occurred. Retry attempt: {Attempt}, trying again in {Seconds} seconds. StackTrace: {StackTrace}"
                , exception.GetType().Name
                , exception.Message
                , attempt
                , TimeSpan.FromSeconds(5)
                , exception.StackTrace);
        });
}