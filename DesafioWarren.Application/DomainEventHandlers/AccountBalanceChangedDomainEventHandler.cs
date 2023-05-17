using System.Threading;
using System.Threading.Tasks;
using DesafioWarren.Application.Hubs;
using DesafioWarren.Application.Services.Cache;
using DesafioWarren.Domain.DomainEvents;
using MediatR;
using Microsoft.AspNetCore.SignalR;

namespace DesafioWarren.Application.DomainEventHandlers
{
    public class AccountBalanceChangedDomainEventHandler : INotificationHandler<AccountBalanceChangedDomainEvent>
    {
        private readonly IHubContext<AccountsHub> _hubContext;

        private readonly ICachedConnectedAccountsManager _accountsManager;

        public AccountBalanceChangedDomainEventHandler(IHubContext<AccountsHub> hubContext, ICachedConnectedAccountsManager accountsManager)
        {
            _hubContext = hubContext;
            _accountsManager = accountsManager;
        }

        public async Task Handle(AccountBalanceChangedDomainEvent notification, CancellationToken cancellationToken)
        {
            var connectedClient = await _accountsManager.GetConnectionIdForAccount(notification.Account.Id);
            
            await _hubContext.Clients.Client(connectedClient).SendCoreAsync("AccountBalanceChanged", new[] { notification.Account.GetBalance() }, cancellationToken);

        }
    }
}