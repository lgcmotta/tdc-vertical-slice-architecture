namespace BankingApp.Domain.Core;

public interface IUnitOfWork
{
    Task<int> SaveAsync(CancellationToken cancellationToken = default);
}