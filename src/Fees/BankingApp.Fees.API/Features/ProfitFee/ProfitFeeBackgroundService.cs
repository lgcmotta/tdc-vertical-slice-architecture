using MediatR;
using Microsoft.Extensions.Options;

namespace BankingApp.Fees.API.Features.ProfitFee;

public class ProfitFeeBackgroundService : BackgroundService
{
    private readonly IServiceProvider _serviceProvider;
    private readonly IOptions<ProfitFeeOptions> _options;
    private readonly PeriodicTimer _timer;

    public ProfitFeeBackgroundService(IServiceProvider serviceProvider, IOptions<ProfitFeeOptions> options)
    {
        _serviceProvider = serviceProvider;
        _options = options;
        _timer = new PeriodicTimer(options.Value.ExecutionInterval);
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (await _timer.WaitForNextTickAsync(stoppingToken) && !stoppingToken.IsCancellationRequested)
        {
            using var scope = _serviceProvider.CreateScope();

            var mediator = scope.ServiceProvider.GetRequiredService<IMediator>();

            var command = new ProfitFeeCommand(_options.Value.Rate, _options.Value.BalanceIdleDays);

            await mediator.Send(command, stoppingToken).ConfigureAwait(continueOnCapturedContext: false);
        }
    }
}