using System;
using DesafioWarren.Domain.ValueObjects;

namespace DesafioWarren.Domain.Entities
{
    public sealed class AccountTransaction : Entity
    {
        private decimal _transactionValue;

        private decimal _balanceBeforeTransaction;
        
        public TransactionType TransactionType { get; private set; }

        public DateTime Occurrence { get; private set; }

        public decimal TransactionValue => SignedTransactionValueFactory.GetTransactionValueWithSignal(_transactionValue, TransactionType);

        public decimal BalanceBeforeTransaction => _balanceBeforeTransaction;

        public AccountTransaction(TransactionType transactionType, DateTime occurrence, decimal transactionValue, decimal balanceBeforeTransaction)
        {
            TransactionType = transactionType;
            Occurrence = occurrence;
            _transactionValue = transactionValue;
            _balanceBeforeTransaction = balanceBeforeTransaction;
        }

        private AccountTransaction()
        {
                
        }
        
    }
}