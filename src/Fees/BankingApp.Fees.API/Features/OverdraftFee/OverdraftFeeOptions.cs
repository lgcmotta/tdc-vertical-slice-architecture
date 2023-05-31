namespace BankingApp.Fees.API.Features.OverdraftFee;

public class OverdraftFeeOptions
{
    public TimeSpan ExecutionInterval { get; set; }

    public decimal Rate { get; set; }
}