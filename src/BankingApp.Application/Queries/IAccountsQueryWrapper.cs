using BankingApp.Application.Models;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace BankingApp.Application.Queries;

public interface IAccountsQueryWrapper
{
    Task<Response> GetContactsAsync(Guid accountId, CancellationToken cancellationToken = default);

    Task<Response> GetAccountTransactions(Guid accountId, CancellationToken cancellationToken = default);
        
    Task<Response> GetMyselfAsync();
}