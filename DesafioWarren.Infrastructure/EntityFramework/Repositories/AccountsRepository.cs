using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using DesafioWarren.Domain.Aggregates;
using DesafioWarren.Domain.Entities;
using DesafioWarren.Domain.Repositories;
using DesafioWarren.Domain.UnitOfWork;
using DesafioWarren.Infrastructure.EntityFramework.DbContexts;
using Microsoft.EntityFrameworkCore;

namespace DesafioWarren.Infrastructure.EntityFramework.Repositories
{
    public class AccountsRepository : IAccountRepository
    {
        private readonly AccountsDbContext _context;

        public IUnitOfWork UnitOfWork => _context;
        
        public AccountsRepository(AccountsDbContext context)
        {
            _context = context;
        }

        public void Add(Account account)
        {
            _context.Set<Account>().Add(account);
        }

        public void AddRange(IEnumerable<Account> accounts)
        {
            _context.Set<Account>().AddRange(accounts);
        }

        public void Remove(Account account)
        {
            _context.Set<Account>().Remove(account);
        }

        public void RemoveRange(IEnumerable<Account> accounts)
        {
            _context.Set<Account>().RemoveRange(accounts);
        }

        public async Task<Account> GetAccountByIdAsync(Guid accountId, CancellationToken cancellationToken = default)
        {
            return await _context.Set<Account>()
                .Include("_accountBalance")
                .FirstOrDefaultAsync(account => account.Id == accountId, cancellationToken: cancellationToken);
        }

        public async Task<Account> GetAccountByNumberAsync(string accountNumber, CancellationToken cancellationToken = default)
        {
            return await _context.Set<Account>()
                .Include("_accountBalance")
                .FirstOrDefaultAsync(account => account.Number == accountNumber, cancellationToken);
        }

        public async Task<IEnumerable<Account>> GetAccountsExceptAsync(Guid accountId, CancellationToken cancellationToken = default)
        {
            return await _context.Set<Account>()
                .Include("_accountBalance")
                .Where(account => account.Id != accountId)
                .ToListAsync(cancellationToken);
        }

        public async Task<IEnumerable<AccountTransaction>> GetAccountTransactionsAsync(Guid accountId, CancellationToken cancellationToken = default)
        {
            return await _context.Set<AccountTransaction>()
                .FromSqlInterpolated(
                    $"SELECT AccountTransaction.Id AS Id, TransactionType, AccountBalanceId, TransactionValue, BalanceBeforeTransaction, Occurrence FROM AccountTransaction INNER JOIN AccountBalance AB on AccountTransaction.AccountBalanceId = AB.Id INNER JOIN Accounts A on AB.AccountId = A.Id WHERE A.Id = {accountId}")
                .ToListAsync(cancellationToken);
        }

        public async Task<Account> GetAccountByNameAsync(string name, CancellationToken cancellationToken = default)
        {
            return await _context.Set<Account>()
                .Include("_accountBalance")
                .FirstOrDefaultAsync(account => account.Name == name, cancellationToken);
        }

        public async Task<Account> GetAccountByCpf(string cpf, CancellationToken cancellationToken = default)
        {
            return await _context.Set<Account>()
                .Include("_accountBalance")
                .FirstOrDefaultAsync(account => account.Cpf == cpf, cancellationToken);
        }

        public async Task<IEnumerable<Account>> GetAccountsAsync(CancellationToken cancellationToken = default)
        {
            return await _context.Set<Account>()
                .Include("_accountBalance")
                .ToListAsync(cancellationToken);
        }
    }
}