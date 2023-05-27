using BankingApp.Domain.Core;
using BankingApp.Infrastructure.Core.Persistence;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace BankingApp.Application.Core.Behaviors;


public class ResilientTransactionBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IBaseRequest, ICommand
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMediator _mediator;

    public ResilientTransactionBehavior(IUnitOfWork unitOfWork, IMediator mediator)
    {
        _unitOfWork = unitOfWork;
        _mediator = mediator;
    }

    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        var strategy = _unitOfWork.CreateExecutionStrategy();

        var response = await strategy.ExecuteAsync(async () => await TryExecuteStrategy(next, cancellationToken)
                .ConfigureAwait(continueOnCapturedContext: false))
            .ConfigureAwait(continueOnCapturedContext: false);

        return response;
    }

    private async Task<TResponse> TryExecuteStrategy(RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken = default)
    {
        try
        {
            await _unitOfWork.BeginTransactionAsync(cancellationToken)
                .ConfigureAwait(continueOnCapturedContext: false);

            var response = await next();

            await _unitOfWork.SaveChangesAsync(cancellationToken)
                .ConfigureAwait(continueOnCapturedContext: false);

            var domainEvents = _unitOfWork.ExtractDomainEventsFromAggregates();

            await _unitOfWork.SaveChangesAsync(cancellationToken)
                .ConfigureAwait(continueOnCapturedContext: false);

            await _unitOfWork.CommitTransactionAsync(cancellationToken)
                .ConfigureAwait(continueOnCapturedContext: false);

            var dispatchingTasks = domainEvents.Select(domainEvent => _mediator.Publish(domainEvent, cancellationToken));

            await Task.WhenAll(dispatchingTasks);

            return response;
        }
        catch
        {
            await _unitOfWork.RollBackTransactionAsync(cancellationToken)
                .ConfigureAwait(continueOnCapturedContext: false);

            throw;
        }
    }
}