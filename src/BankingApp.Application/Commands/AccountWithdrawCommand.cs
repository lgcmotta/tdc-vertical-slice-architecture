using BankingApp.Domain.ValueObjects;
using System;

namespace BankingApp.Application.Commands;

public class AccountWithdrawCommand : FinancialOperationCommand
{
    public AccountWithdrawCommand(Guid accountId, decimal value) : base(accountId, value)
    {
    }

    public override TransactionType GetTransactionType() => TransactionType.Withdraw;
}