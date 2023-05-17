using System;
using System.Threading;
using System.Threading.Tasks;
using DesafioWarren.Infrastructure.EntityFramework.DbContexts;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Serilog;

namespace DesafioWarren.Application.Behaviours
{
    public class TransactionalBehaviour<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    {
        private readonly AccountsDbContext _context;

        private readonly ILogger _logger;

        public TransactionalBehaviour(AccountsDbContext context, ILogger logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken, RequestHandlerDelegate<TResponse> next)
        {
            try
            {
                var response = default(TResponse);

                if (_context.HasActiveTransaction) return await next();

                _logger.Information("Creating execution strategy for database.");

                var strategy = _context.Database.CreateExecutionStrategy();

                await strategy.ExecuteAsync(async () =>
                {
                    _logger.Information("Database transaction started.");
                    
                    await using var transaction = await _context.BeginTransactionAsync(cancellationToken);
                    
                    response = await next();

                    await _context.CommitTransactionAsync(transaction, cancellationToken);

                    _logger.Information("Database transaction committed.");
                });

                return response;
            }
            catch (Exception exception)
            {

                _logger.Error(exception, "An exception occurred when executing the transactional behaviour.");
                
                return default;
            }
        }
    }
}