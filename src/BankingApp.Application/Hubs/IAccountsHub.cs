using System;
using System.Threading.Tasks;

namespace BankingApp.Application.Hubs;

public interface IAccountsHub
{
    public Task AppendAccountToList(Guid accountId, string connectionId);
}