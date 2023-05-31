namespace BankingApp.Fees.API.Features.ProfitFee;

public class ProfitFeeOptions
{
    public TimeSpan ExecutionInterval { get; set; }

    public int BalanceIdleInMinutes { get; set; }

    public decimal Rate { get; set; }
}