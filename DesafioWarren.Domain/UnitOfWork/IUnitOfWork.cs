using System.Threading;
using System.Threading.Tasks;

namespace DesafioWarren.Domain.UnitOfWork
{
    public interface IUnitOfWork
    {
        Task<bool> SaveEntitiesAsync(CancellationToken cancellationToken = default);
    }
}