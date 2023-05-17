using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.Internal;
using DesafioWarren.Application.Models;
using DesafioWarren.Application.Services.Identity;
using DesafioWarren.Domain.Repositories;
using DesafioWarren.Domain.ValueObjects;
using Microsoft.Extensions.Configuration;

namespace DesafioWarren.Application.Queries
{
    public class AccountsQueryWrapper : IAccountsQueryWrapper
    {
        private readonly IAccountRepository _accountRepository;

        private readonly IMapper _mapper;

        private readonly IIdentityService _identityService;

        private readonly decimal _earningsTaxPerDay;

        public AccountsQueryWrapper(IAccountRepository accountRepository, IMapper mapper, IIdentityService identityService, IConfiguration configuration)
        {
            _accountRepository = accountRepository;
            _mapper = mapper;
            _identityService = identityService;
            _earningsTaxPerDay = configuration.GetValue<decimal>("EarningsPerDayTax");
        }

        public async Task<Response> GetContactsAsync(Guid accountId, CancellationToken cancellationToken = default)
        {
            var accounts = await _accountRepository.GetAccountsExceptAsync(accountId, cancellationToken);

            var accountsModels = accounts.Select(account => new AccountModelBase
            {
                Name = account.Name
                , Cpf = account.Cpf
                , Email = account.Email
                , PhoneNumber = account.PhoneNumber
                , Currency = account.GetCurrencyIsoCode()
                , Number =  account.Number
            });

            return new Response(accountsModels);
        }

        public async Task<Response> GetAccountTransactions(Guid accountId, CancellationToken cancellationToken = default)
        {
            var transactions = await _accountRepository.GetAccountTransactionsAsync(accountId, cancellationToken);

            var transactionsModels = _mapper.Map<IEnumerable<AccountTransactionModel>>(transactions).ToList();

            transactionsModels.ForAll(transactionsModel => transactionsModel.EarningsTaxPerDay = transactionsModel.TransactionType == TransactionType.Earnings.Value ? _earningsTaxPerDay : null);

            return new Response(transactionsModels);
        }

        public async Task<Response> GetMyselfAsync()
        {
            var name = _identityService.GetUserDisplayName();

            var account = await _accountRepository.GetAccountByNameAsync(name);

            return new Response(_mapper.Map<AccountModel>(account));
        }
    }
}