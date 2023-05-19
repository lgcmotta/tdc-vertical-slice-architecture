using BankingApp.Domain.ValueObjects;
using System;

namespace BankingApp.Application.Commands;

public class AccountPaymentCommand : FinancialOperationCommand
{
    public string InvoiceNumber { get; set; }
        
    public AccountPaymentCommand(Guid accountId, decimal value, string invoiceNumber) : base(accountId, value)
    {
        InvoiceNumber = invoiceNumber;
    }

    public override TransactionType GetTransactionType() => TransactionType.Payment;
}