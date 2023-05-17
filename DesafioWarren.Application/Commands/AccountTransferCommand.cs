using System;
using DesafioWarren.Domain.ValueObjects;

namespace DesafioWarren.Application.Commands
{
    public class AccountTransferCommand : FinancialOperationCommand
    {
        public string DestinationAccount { get; set; }

        public AccountTransferCommand(Guid accountId, decimal value, string destinationAccount) : base(accountId, value)
        {
            DestinationAccount = destinationAccount;
        }

        public override TransactionType GetTransactionType() => TransactionType.Transfer;
    }
}