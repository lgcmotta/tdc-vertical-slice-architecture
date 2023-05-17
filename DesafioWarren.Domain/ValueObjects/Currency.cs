namespace DesafioWarren.Domain.ValueObjects
{
    public class Currency : Enumeration
    {
        public string Symbol { get; }

        public static Currency BrazilianReal => new(0, "BRL", "R$");

        public static Currency Dollar => new(1, "USD", "$");

        public static Currency Euro => new(2, "EUR", "€");

        public static Currency BritishPound => new(3, "GBP", "£");

        
        public static Currency UruguayanPeso = new(4, "UYU", "$");

        public Currency(int id, string value, string symbol) : base(id, value)
        {
            Symbol = symbol;
        }
    }
}