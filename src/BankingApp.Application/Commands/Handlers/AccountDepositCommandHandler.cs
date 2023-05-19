﻿using BankingApp.Application.Models;
using BankingApp.Domain.Repositories;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace BankingApp.Application.Commands.Handlers;

public class AccountDepositCommandHandler : IRequestHandler<AccountDepositCommand, Response>
{
    private readonly IAccountRepository _accountRepository;

    public AccountDepositCommandHandler(IAccountRepository accountRepository)
    {
        _accountRepository = accountRepository;
    }

    public async Task<Response> Handle(AccountDepositCommand request, CancellationToken cancellationToken)
    {
        var account = await _accountRepository.GetAccountByIdAsync(request.AccountId, cancellationToken);

        account.Deposit(request.Value);

        account.AddAccountBalanceChangedDomainEvent();

        var transactionResult = new TransactionResult("OK"
            , DateTime.Now
            , $"{request.GetTransactionType().Value} of {account.GetCurrencySymbol()}{request.Value} was successfully made.");
            
        return new Response(transactionResult);
    }
}