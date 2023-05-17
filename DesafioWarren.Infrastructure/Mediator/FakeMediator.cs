using System.Threading;
using System.Threading.Tasks;
using MediatR;

namespace DesafioWarren.Infrastructure.Mediator
{
    public class FakeMediator : IMediator
    {
        public Task<TResponse> Send<TResponse>(IRequest<TResponse> request
            , CancellationToken cancellationToken = new()) =>
            Task.CompletedTask as Task<TResponse>;

        public Task<object?> Send(object request
            , CancellationToken cancellationToken = new()) =>
            Task.FromResult<object>(default);

        public Task Publish(object notification
            , CancellationToken cancellationToken = new()) =>
            Task.CompletedTask;

        public Task Publish<TNotification>(TNotification notification
            , CancellationToken cancellationToken = new()) where TNotification : INotification =>
            Task.CompletedTask;
    }
}