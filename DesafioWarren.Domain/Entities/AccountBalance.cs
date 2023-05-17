using System;
using System.Collections.Generic;
using DesafioWarren.Domain.Aggregates;
using DesafioWarren.Domain.ValueObjects;

namespace DesafioWarren.Domain.Entities
{
    public sealed class AccountBalance : Entity
    {
        private decimal _balance;

        private Currency _currency;

        private DateTime _lastModified;

        private ICollection<AccountTransaction> _transactions = new List<AccountTransaction>();
        
        public IEnumerable<AccountTransaction> Transactions => _transactions;

        public Currency Currency => _currency;

        public decimal Balance => _balance;

        public DateTime LastModified    
        {
            get => _lastModified;
            private set => _lastModified = value;
        }

        public AccountBalance(Currency currency)
        {
            _balance = 0;
            _currency = currency;
            _lastModified = DateTime.Now;
        }

        public AccountBalance()
        {
            _balance = 0;
            _currency = Currency.BrazilianReal;
        }
        
        private void AddTransaction(TransactionType transactionType, decimal transactionValue, decimal balanceBeforeTransaction)
        {
            var now = DateTime.Now;

            _transactions.Add(new AccountTransaction(transactionType, now, transactionValue, balanceBeforeTransaction));

            _lastModified = now;
        }

        public void Deposit(decimal value)
        {
            var backupBalance = _balance;

            try
            {
                _balance += value;

                AddTransaction(TransactionType.Deposit, value, backupBalance);
            }
            catch
            {
                _balance = backupBalance;
            }
        }

        public void Transfer(Account destination, decimal value)
        {
            var backupBalance = _balance;

            try
            {
                _balance -= value;

                destination.Deposit(value);

                AddTransaction(TransactionType.Transfer, value, backupBalance);
            }
            catch
            {
                _balance = backupBalance;
            }
        }

        public void Payment(decimal value)
        {
            var backupBalance = _balance;

            try
            {
                _balance -= value;

                AddTransaction(TransactionType.Payment, value, backupBalance);
            }
            catch
            {
                _balance = backupBalance;
            }
        }

        public decimal Withdraw(decimal value)
        {
            var backupBalance = _balance;

            try
            {
                _balance -= value;

                AddTransaction(TransactionType.Withdraw, value, backupBalance);

                return _balance;
            }
            catch
            {
                _balance = backupBalance;

                return 0;
            }
        }
        public void Earnings(decimal value)
        {
            var backupBalance = _balance;

            try
            {
                _balance += value;

                AddTransaction(TransactionType.Earnings, value, backupBalance);

            }
            catch
            {
                _balance = backupBalance;
            }
        }

        public void DefineCurrency(string isoCode) => _currency = Enumeration.GetItemByValue<Currency>(isoCode);

        
    }
}