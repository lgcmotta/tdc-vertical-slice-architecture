using MediatR;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace BankingApp.Infrastructure.Mediator;

public class NullMediator : IMediator
{
    public Task<TResponse> Send<TResponse>(IRequest<TResponse> request, CancellationToken cancellationToken = default) =>
        Task.FromResult(default(TResponse));

    public Task Send<TRequest>(TRequest request, CancellationToken cancellationToken = default) where TRequest : IRequest =>
        Task.CompletedTask;

    public Task<object> Send(object request, CancellationToken cancellationToken = default) =>
        Task.FromResult(default(object));

    public IAsyncEnumerable<TResponse> CreateStream<TResponse>(IStreamRequest<TResponse> request,
        CancellationToken cancellationToken = default) => default;

    public IAsyncEnumerable<object> CreateStream(object request, CancellationToken cancellationToken = default) => default;

    public Task Publish(object notification, CancellationToken cancellationToken = default) => Task.CompletedTask;

    public Task Publish<TNotification>(TNotification notification, CancellationToken cancellationToken = default)
        where TNotification : INotification => Task.CompletedTask;
}