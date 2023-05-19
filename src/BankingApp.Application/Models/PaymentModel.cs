namespace BankingApp.Application.Models;

public class PaymentModel : TransactionModel
{
    public string InvoiceNumber { get; set; }
}