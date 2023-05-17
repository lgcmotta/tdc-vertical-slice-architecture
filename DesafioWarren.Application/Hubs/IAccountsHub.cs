using System;
using System.Threading.Tasks;

namespace DesafioWarren.Application.Hubs
{
    public interface IAccountsHub
    {
        public Task AppendAccountToList(Guid accountId, string connectionId);
    }
}