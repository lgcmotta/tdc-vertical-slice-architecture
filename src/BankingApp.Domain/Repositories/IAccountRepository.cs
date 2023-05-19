using BankingApp.Domain.Aggregates;
using BankingApp.Domain.Entities;
using BankingApp.Domain.UnitOfWork;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace BankingApp.Domain.Repositories;

public interface IAccountRepository
{
    IUnitOfWork UnitOfWork { get; }

    void Add(Account account);

    void AddRange(IEnumerable<Account> accounts);

    void Remove(Account account);

    void RemoveRange(IEnumerable<Account> accounts);

    Task<Account> GetAccountByIdAsync(Guid accountId, CancellationToken cancellationToken = default);
        
    Task<Account> GetAccountByNumberAsync(string accountNumber, CancellationToken cancellationToken = default);
        
    Task<IEnumerable<Account>> GetAccountsExceptAsync(Guid accountId, CancellationToken cancellationToken = default);

    Task<IEnumerable<AccountTransaction>> GetAccountTransactionsAsync(Guid accountId, CancellationToken cancellationToken = default);
        
    Task<Account> GetAccountByNameAsync(string name, CancellationToken cancellationToken = default);
        
    Task<Account> GetAccountByCpf(string cpf, CancellationToken cancellationToken = default);
        
    Task<IEnumerable<Account>> GetAccountsAsync(CancellationToken cancellationToken = default);
}