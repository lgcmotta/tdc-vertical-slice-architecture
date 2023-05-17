using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Distributed;

namespace DesafioWarren.Application.Services.Cache
{
    public class CachedConnectedAccountsManager : ICachedConnectedAccountsManager
    {
        private readonly IDistributedCache _distributedCache;

        public CachedConnectedAccountsManager(IDistributedCache distributedCache)
        {
            _distributedCache = distributedCache;
        }


        public async Task AppendAccountId(Guid accountId, string connectionId)
        {
            await _distributedCache.SetStringAsync(accountId.ToString(), connectionId);
        }

        public async Task<string> GetConnectionIdForAccount(Guid accountId)
        {
            return await _distributedCache.GetStringAsync(accountId.ToString());
        }
    }
}