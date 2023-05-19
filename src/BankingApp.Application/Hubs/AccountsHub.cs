using BankingApp.Application.Services.Cache;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Threading.Tasks;

namespace BankingApp.Application.Hubs;

public class AccountsHub : Hub
{
    private readonly ICachedConnectedAccountsManager _accountsManager;

    public AccountsHub(ICachedConnectedAccountsManager accountsManager)
    {
        _accountsManager = accountsManager;
    }

    public override async Task OnConnectedAsync()
    {
        await base.OnConnectedAsync();
    }

    public override async Task OnDisconnectedAsync(Exception exception)
    {
        await base.OnDisconnectedAsync(exception);
    }
        
    public Task AppendAccountToList(Guid accountId, string connectionId)
    {
        _accountsManager.AppendAccountId(accountId, connectionId);

        return Task.CompletedTask;
    }
}