﻿using BankingApp.Application.Models;
using BankingApp.Domain.Repositories;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace BankingApp.Application.Commands.Handlers;

public class AccountTransferCommandHandler : IRequestHandler<AccountTransferCommand, Response>
{
    private readonly IAccountRepository _accountRepository;

    public AccountTransferCommandHandler(IAccountRepository accountRepository)
    {
        _accountRepository = accountRepository;
    }

    public async Task<Response> Handle(AccountTransferCommand request, CancellationToken cancellationToken)
    {
        var account = await _accountRepository.GetAccountByIdAsync(request.AccountId, cancellationToken);

        var destinationAccount =
            await _accountRepository.GetAccountByNumberAsync(request.DestinationAccount, cancellationToken);

        account.Transfer(destinationAccount, request.Value);

        account.AddAccountBalanceChangedDomainEvent();

        await _accountRepository.UnitOfWork.SaveEntitiesAsync(cancellationToken);

        var transactionResult = new TransactionResult("OK"
            , DateTime.Now
            , $"{request.GetTransactionType().Value} of {account.GetCurrencySymbol()}{request.Value} to {destinationAccount.Name} was successfully made.");

        return new Response(transactionResult);
    }
}