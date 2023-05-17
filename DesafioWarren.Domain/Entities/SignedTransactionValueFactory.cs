using System.Collections.Generic;
using System.Linq;
using DesafioWarren.Domain.ValueObjects;

namespace DesafioWarren.Domain.Entities
{
    internal sealed class SignedTransactionValueFactory
    {
        private static readonly IEnumerable<(int Multiplier, TransactionType TransactionType)> TransactionTypeMultipliers = new[]
        {
            (1, TransactionType.Deposit)
            , (-1, TransactionType.Payment)     
            , (-1, TransactionType.Transfer)
            , (-1, TransactionType.Withdraw)
            , (1, TransactionType.Earnings)
        };  


        public static decimal GetTransactionValueWithSignal(decimal value, TransactionType transactionType)
        {
            if (value == 0) return value;

            var multiplier = TransactionTypeMultipliers.FirstOrDefault(signalType => signalType.TransactionType == transactionType).Multiplier;

            return value * multiplier;
        }
    }
}