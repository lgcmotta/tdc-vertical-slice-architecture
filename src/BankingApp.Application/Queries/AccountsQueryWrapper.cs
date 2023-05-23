﻿using AutoMapper;
using BankingApp.Application.Models;
using BankingApp.Domain.Repositories;
using BankingApp.Domain.ValueObjects;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace BankingApp.Application.Queries;

public class AccountsQueryWrapper : IAccountsQueryWrapper
{
    private readonly IAccountRepository _accountRepository;
    private readonly IMapper _mapper;
    private readonly decimal _earningsTaxPerDay;

    public AccountsQueryWrapper(IAccountRepository accountRepository, IMapper mapper, IConfiguration configuration)
    {
        _accountRepository = accountRepository;
        _mapper = mapper;
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

        transactionsModels.ForEach(transactionsModel => transactionsModel.EarningsTaxPerDay = transactionsModel.TransactionType == TransactionType.Earnings.Value ? _earningsTaxPerDay : null);

        return new Response(transactionsModels);
    }
}