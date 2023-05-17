namespace DesafioWarren.Domain.ValueObjects
{
    public class TransactionType : Enumeration
    {
        public static TransactionType Transfer => new(0, "Transfer");   

        public static TransactionType Deposit => new(1, "Deposit");

        public static TransactionType Payment => new(2, "Payment");

        public static TransactionType Withdraw => new(3, "Withdraw");

        public static TransactionType Earnings => new(4, "Earnings");
        

        public TransactionType(int id, string value) : base(id, value)
        {
        }
    }
}