namespace BankingApp.Fees.API.Features.ProfitFee;

public class ProfitFeeOptions
{
    public TimeSpan ExecutionInterval { get; set; }

    public int BalanceIdleDays { get; set; }

    public decimal Rate { get; set; }
}