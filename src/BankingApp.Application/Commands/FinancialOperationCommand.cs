using BankingApp.Domain.ValueObjects;
using System;

namespace BankingApp.Application.Commands;

public abstract class FinancialOperationCommand : AccountIdCommand
{
    public decimal Value { get; }

    protected FinancialOperationCommand(Guid accountId, decimal value) : base(accountId)
    {
        Value = value;
    }

    public abstract TransactionType GetTransactionType();
}