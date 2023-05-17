using System.Threading;
using System.Threading.Tasks;
using DesafioWarren.Application.Extensions;
using MediatR;
using Serilog;


namespace DesafioWarren.Application.Behaviours
{
    public class LoggingBehaviour<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    {
        private readonly ILogger _logger;

        public LoggingBehaviour(ILogger logger)
        {
            _logger = logger;
        }


        public async Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken, RequestHandlerDelegate<TResponse> next)
        {
            var commandName = request.GetGenericTypeName();

            _logger.Information("Handling command '{CommandName}'", commandName);

            var response = await next();

            _logger.Information("Command '{CommandName}' was handled successfully.", commandName);

            return response;
        }
    }
}