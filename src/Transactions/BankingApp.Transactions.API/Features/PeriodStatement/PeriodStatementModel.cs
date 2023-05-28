namespace BankingApp.Transactions.API.Features.PeriodStatement;

public class PeriodStatementModel
{
    public Guid TransactionId { get; set; }
    public decimal Value { get; set; }
    public string FormattedValue { get; set; }
    public decimal PreviousBalance { get; set; }
    public string FormattedPreviousBalance { get; set; }
    public string Type { get; set; }
    public string SenderToken { get; set; }
    public string SenderName { get; set; }
    public string ReceiverToken { get; set; }
    public string ReceiverName { get; set; }
    public DateTime Occurrence { get; set; }
}