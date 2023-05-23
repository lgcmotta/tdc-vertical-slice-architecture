using BankingApp.Application.Models;
using BankingApp.Domain.Repositories;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace BankingApp.Application.Commands.Handlers;

public class AccountWithdrawCommandHandler : IRequestHandler<AccountWithdrawCommand, Response>
{
    private readonly IAccountRepository _accountRepository;

    public AccountWithdrawCommandHandler(IAccountRepository accountRepository)
    {
        _accountRepository = accountRepository;
    }

    public async Task<Response> Handle(AccountWithdrawCommand request, CancellationToken cancellationToken)
    {
        var account = await _accountRepository.GetAccountByIdAsync(request.AccountId, cancellationToken);

        account.Withdraw(request.Value);

        account.AddAccountBalanceChangedDomainEvent();

        await _accountRepository.UnitOfWork.SaveEntitiesAsync(cancellationToken);

        var transactionResult = new TransactionResult("OK"
            , DateTime.Now
            , $"{request.GetTransactionType().Value} of {account.GetCurrencySymbol()}{request.Value} was successfully made.");

        return new Response(transactionResult);
    }
}