using BankingApp.Domain.ValueObjects;
using System;

namespace BankingApp.Application.Commands;

public class AccountTransferCommand : FinancialOperationCommand
{
    public string DestinationAccount { get; set; }

    public AccountTransferCommand(Guid accountId, decimal value, string destinationAccount) : base(accountId, value)
    {
        DestinationAccount = destinationAccount;
    }

    public override TransactionType GetTransactionType() => TransactionType.Transfer;
}