using System;
using System.Collections.Generic;
using DesafioWarren.Domain.DomainEvents;
using DesafioWarren.Domain.Entities;
using DesafioWarren.Domain.ValueObjects;

namespace DesafioWarren.Domain.Aggregates
{
    public class Account : Entity, IAggregateRoot
    {
        private string _name;

        private string _email;
        
        private string _phoneNumber;

        private string _accountNumber;

        private AccountBalance _accountBalance;
        
        private DateTime _lastModified;

        public string Cpf { get; private set; }
        
        public string Name => _name;

        public string Email => _email;

        public string PhoneNumber => _phoneNumber;

        public string Number => _accountNumber;

        public DateTime LastModified
        {
            get => _lastModified;
            private set => _lastModified = value;
        }

        public void ChangeEmail(string email) => _email = email;

        public void ChangePhoneNumber(string phoneNumber) => _phoneNumber = phoneNumber;
        
        private Account() { }

        public Account(string name, string email, string phoneNumber, string cpf, Currency currency)
        {
            Id = Guid.NewGuid();
            _name = name;
            _email = email;
            _phoneNumber = phoneNumber;
            Cpf = cpf;
            _accountBalance = new AccountBalance(currency);
            _lastModified = DateTime.Now;
        }

        public void Deposit(decimal value)
        {
            _accountBalance.Deposit(value);
            
            _lastModified = _accountBalance.LastModified;
        }

        public void Transfer(Account destination, decimal value)
        {
            _accountBalance.Transfer(destination, value);

            _lastModified = _accountBalance.LastModified;
        }

        public void Payment(decimal value)
        {
            _accountBalance.Payment(value);

            _lastModified = _accountBalance.LastModified;
        }

        public decimal Withdraw(decimal value)
        {
            var withdrawnValue = _accountBalance.Withdraw(value);

            _lastModified = _accountBalance.LastModified;

            return withdrawnValue;
        }

        public void Earnings(decimal value)
        {
            _accountBalance.Earnings(value);

            _lastModified = _accountBalance.LastModified;
        }

        public string GetBalance() => $"{_accountBalance.Currency.Symbol}{decimal.Round(_accountBalance.Balance, 2, MidpointRounding.AwayFromZero):F}";

        public decimal GetBalanceValue() => _accountBalance.Balance; 

        public IEnumerable<AccountTransaction> GetAccountTransactions() => _accountBalance.Transactions;

        public void CorrectCpf(string cpf) => Cpf = cpf;

        public void CorrectName(string name) => _name = name;

        public string GetCurrencySymbol() => _accountBalance.Currency.Symbol;

        public string GetCurrencyIsoCode() => _accountBalance.Currency.Value;

        public void DefineCurrency(string isoCode) => _accountBalance.DefineCurrency(isoCode);

        public void AddAccountBalanceChangedDomainEvent()
        {
            AddDomainEvent(new AccountBalanceChangedDomainEvent(this));
        }
    }
}