using System.Threading.Tasks;
using DesafioWarren.Application.Commands;
using MediatR;
using Quartz;
using Serilog;

namespace DesafioWarren.Application.Quartz
{
    public class AccountsEarningJob : IJob
    {
        private readonly IMediator _mediator;
        
        private readonly ILogger _logger;

        public AccountsEarningJob(IMediator mediator, ILogger logger)
        {
            _mediator = mediator;
            _logger = logger;
        }

        public async Task Execute(IJobExecutionContext context)
        {
            _logger.Information("Accounts earnings job calculator started");

            var response = await _mediator.Send(new CalculateAccountEarningsCommand());

            _logger.Information("Accounts earnings job calculator ended with {Failures} failures", response.Failures.Count);
        }
    }
}