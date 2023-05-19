using BankingApp.Domain.ValueObjects;
using System;

namespace BankingApp.Application.Commands;

public class AccountDepositCommand : FinancialOperationCommand
{
    public AccountDepositCommand(Guid accountId, decimal value) : base(accountId, value)
    {
    }


    public override TransactionType GetTransactionType() => TransactionType.Deposit;
}