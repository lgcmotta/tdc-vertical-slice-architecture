using System;
using System.Threading.Tasks;

namespace DesafioWarren.Application.Services.Cache
{
    public interface ICachedConnectedAccountsManager
    {
        Task AppendAccountId(Guid accountId, string connectionId);
            
        Task<string> GetConnectionIdForAccount(Guid accountId);
    }
}       