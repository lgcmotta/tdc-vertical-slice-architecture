using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using DesafioWarren.Application.Models;
using DesafioWarren.Domain.Repositories;
using MediatR;
using Microsoft.Extensions.Configuration;

namespace DesafioWarren.Application.Commands.Handlers
{
    public class CalculateAccountEarningsCommandHandler : IRequestHandler<CalculateAccountEarningsCommand, Response>
    {
        private readonly IAccountRepository _accountRepository;

        private readonly decimal _earningsPerDayTax;

        public CalculateAccountEarningsCommandHandler(IAccountRepository accountRepository, IConfiguration configuration)
        {
            _accountRepository = accountRepository;

            _earningsPerDayTax = configuration.GetValue<decimal>("EarningsPerDayTax");
        }

            
        public async Task<Response> Handle(CalculateAccountEarningsCommand request, CancellationToken cancellationToken)
        {
            var accounts = await _accountRepository.GetAccountsAsync(cancellationToken);

            var accountsList = accounts.ToList();

            foreach (var account in accountsList)
            {
                if (DateTime.Now.Subtract(account.LastModified).TotalHours < 24) continue;

                var balance = account.GetBalanceValue();

                var earnings = balance * _earningsPerDayTax - balance;

                account.Earnings(earnings);

                account.AddAccountBalanceChangedDomainEvent();
            }

            await _accountRepository.UnitOfWork.SaveEntitiesAsync(cancellationToken);

            return new Response();
        }
    }
}