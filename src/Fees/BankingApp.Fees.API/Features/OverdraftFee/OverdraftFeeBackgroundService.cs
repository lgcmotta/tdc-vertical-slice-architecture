using MediatR;
using Microsoft.Extensions.Options;
using System.Diagnostics;

namespace BankingApp.Fees.API.Features.OverdraftFee;

public class OverdraftFeeBackgroundService : BackgroundService
{
    private readonly IServiceProvider _serviceProvider;
    private readonly IOptions<OverdraftFeeOptions> _options;
    private readonly PeriodicTimer _timer;
    private readonly ActivitySource _activitySource;

    public OverdraftFeeBackgroundService(IServiceProvider serviceProvider, IOptions<OverdraftFeeOptions> options)
    {
        _serviceProvider = serviceProvider;
        _options = options;
        _timer = new PeriodicTimer(options.Value.ExecutionInterval);
        _activitySource = new ActivitySource(nameof(OverdraftFeeBackgroundService));
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (await _timer.WaitForNextTickAsync(stoppingToken) && !stoppingToken.IsCancellationRequested)
        {
            using var scope = _serviceProvider.CreateScope();

            var mediator = scope.ServiceProvider.GetRequiredService<IMediator>();

            using var activity = _activitySource.StartActivity(nameof(ExecuteAsync), ActivityKind.Server);

            var command = new OverdraftFeeCommand(_options.Value.Rate);

            await mediator.Send(command, stoppingToken).ConfigureAwait(continueOnCapturedContext: false);
        }
    }
}