using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Storage;

namespace DesafioWarren.Infrastructure.EntityFramework.DbContexts
{
    public interface IDbContext
    {
        bool HasActiveTransaction { get; }

        IDbContextTransaction CurrentTransaction { get; }

        Task<IDbContextTransaction> BeginTransactionAsync(CancellationToken cancellationToken = default);

        Task CommitTransactionAsync(IDbContextTransaction transaction, CancellationToken cancellationToken = default);

        Task RollbackTransactionAsync(CancellationToken cancellationToken = default);
    }
}