using System.Threading;
using System.Threading.Tasks;

namespace BankingApp.Domain.UnitOfWork;

public interface IUnitOfWork
{
    Task<bool> SaveEntitiesAsync(CancellationToken cancellationToken = default);
}