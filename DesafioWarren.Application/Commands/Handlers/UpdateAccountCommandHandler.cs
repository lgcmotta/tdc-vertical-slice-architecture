using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using DesafioWarren.Application.Models;
using DesafioWarren.Domain.Aggregates;
using DesafioWarren.Domain.Repositories;
using MediatR;

namespace DesafioWarren.Application.Commands.Handlers
{
    public class UpdateAccountCommandHandler : IRequestHandler<UpdateAccountCommand, Response>
    {
        private readonly IAccountRepository _accountRepository;

        private readonly IMapper _mapper;

        public UpdateAccountCommandHandler(IAccountRepository accountRepository, IMapper mapper)
        {
            _accountRepository = accountRepository;
            _mapper = mapper;
        }

        public async Task<Response> Handle(UpdateAccountCommand request, CancellationToken cancellationToken)
        {
            var account = await _accountRepository.GetAccountByIdAsync(request.AccountId, cancellationToken);

            ChangeCurrencyIfNecessary(request, account);
            
            CorrectCpfIfNecessary(request, account);

            CorrectNameIfNecessary(request, account);

            ChangeEmailIfNecessary(request, account);

            ChangePhoneNumberIfNecessary(request, account);

            await _accountRepository.UnitOfWork.SaveEntitiesAsync(cancellationToken);

            var accountModel = _mapper.Map<AccountModel>(account);

            return new Response(accountModel);
        }

        private static void ChangeCurrencyIfNecessary(UpdateAccountCommand request, Account account)
        {
            if(account.GetCurrencyIsoCode() != request.Account.Currency)
                account.DefineCurrency(request.Account.Currency);
        }

        private static void ChangePhoneNumberIfNecessary(UpdateAccountCommand request, Account account)
        {
            if (account.PhoneNumber != request.Account.PhoneNumber)
                account.ChangePhoneNumber(request.Account.PhoneNumber);
        }

        private static void ChangeEmailIfNecessary(UpdateAccountCommand request, Account account)
        {
            if (account.Email != request.Account.Email)
                account.ChangeEmail(request.Account.Email);
        }

        private static void CorrectNameIfNecessary(UpdateAccountCommand request, Account account)
        {
            if (account.Name != request.Account.Name)
                account.CorrectName(request.Account.Name);
        }

        private static void CorrectCpfIfNecessary(UpdateAccountCommand request, Account account)
        {
            if (account.Cpf != request.Account.Cpf)
                account.CorrectCpf(request.Account.Cpf);
        }
    }
}