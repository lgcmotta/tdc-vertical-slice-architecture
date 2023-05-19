using BankingApp.Application.Hubs;
using BankingApp.Application.Services.Cache;
using BankingApp.Domain.DomainEvents;
using MediatR;
using Microsoft.AspNetCore.SignalR;
using System.Threading;
using System.Threading.Tasks;

namespace BankingApp.Application.DomainEventHandlers;

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