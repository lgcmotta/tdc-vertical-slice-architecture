using System;
using System.Threading;
using System.Threading.Tasks;
using DesafioWarren.Application.Models;

namespace DesafioWarren.Application.Queries
{
    public interface IAccountsQueryWrapper
    {
        Task<Response> GetContactsAsync(Guid accountId, CancellationToken cancellationToken = default);

        Task<Response> GetAccountTransactions(Guid accountId, CancellationToken cancellationToken = default);
        
        Task<Response> GetMyselfAsync();
    }
}