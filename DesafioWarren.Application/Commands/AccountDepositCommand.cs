using System;
using DesafioWarren.Domain.ValueObjects;

namespace DesafioWarren.Application.Commands
{
    public class AccountDepositCommand : FinancialOperationCommand
    {
        public AccountDepositCommand(Guid accountId, decimal value) : base(accountId, value)
        {
        }


        public override TransactionType GetTransactionType() => TransactionType.Deposit;
    }
}