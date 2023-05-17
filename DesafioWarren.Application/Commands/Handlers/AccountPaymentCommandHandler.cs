using System;
using System.Threading;
using System.Threading.Tasks;
using DesafioWarren.Application.Models;
using DesafioWarren.Domain.Repositories;
using MediatR;

namespace DesafioWarren.Application.Commands.Handlers
{
    public class AccountPaymentCommandHandler : IRequestHandler<AccountPaymentCommand, Response>
    {
        private readonly IAccountRepository _accountRepository;

        public AccountPaymentCommandHandler(IAccountRepository accountRepository)
        {
            _accountRepository = accountRepository;
        }

        public async Task<Response> Handle(AccountPaymentCommand request, CancellationToken cancellationToken)
        {
            var account = await _accountRepository.GetAccountByIdAsync(request.AccountId, cancellationToken);

            account.Payment(request.Value);

            account.AddAccountBalanceChangedDomainEvent();

            await _accountRepository.UnitOfWork.SaveEntitiesAsync(cancellationToken);

            var transactionResult = new TransactionResult("OK"
                , DateTime.Now
                , $"{request.GetTransactionType().Value} of {account.GetCurrencySymbol()}{request.Value} to invoice number {request.InvoiceNumber} was successfully made.");

            return new Response(transactionResult);
        }
    }
}