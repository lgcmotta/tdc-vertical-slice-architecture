using System;
using DesafioWarren.Domain.ValueObjects;

namespace DesafioWarren.Application.Commands
{
    public class AccountWithdrawCommand : FinancialOperationCommand
    {
        public AccountWithdrawCommand(Guid accountId, decimal value) : base(accountId, value)
        {
        }

        public override TransactionType GetTransactionType() => TransactionType.Withdraw;
    }
}