using DesafioWarren.Domain.Aggregates;
using DesafioWarren.Domain.ValueObjects;
using FluentAssertions;
using Xunit;

namespace DesafioWarren.UnitTests.Domain
{
    public class AccountBalanceTests
    {
        [Fact]
        public void NewAccountHasBalanceEqualToZero_Sucess()
        {
            var account = new Account("Foo", "foo@gmail.com", "+5551999999999", "123.456.789.10", Currency.BrazilianReal);

            Assert.Equal($"{Currency.BrazilianReal.Symbol}0", account.GetBalance());
        }

        [Fact]
        public void DepositMoney_Success()
        {
            var account = new Account("Foo", "foo@gmail.com", "+5551999999999", "123.456.789.10", Currency.BrazilianReal);

            account.Deposit(100);

            Assert.Equal($"{Currency.BrazilianReal.Symbol}100", account.GetBalance());

            account.GetAccountTransactions().Should()
                .Contain(transaction => transaction.TransactionType == TransactionType.Deposit);
        }

        [Fact]
        public void TransferMoneyToAnotherAccount_Sucess()
        {
            var account1 = new Account("Foo", "foo@gmail.com", "+5551999999999", "123.456.789.10", Currency.BrazilianReal);
            
            var account2 = new Account("Bar", "bar@gmail.com", "+5551888888888", "098.765.432.10", Currency.BrazilianReal);

            Assert.Equal($"{Currency.BrazilianReal.Symbol}0", account1.GetBalance());
            
            Assert.Equal($"{Currency.BrazilianReal.Symbol}0", account2.GetBalance());

            account1.Deposit(100);

            account1.Transfer(account2, 50);

            Assert.Equal($"{Currency.BrazilianReal.Symbol}50", account1.GetBalance());
            
            Assert.Equal($"{Currency.BrazilianReal.Symbol}50", account2.GetBalance());

            account1.GetAccountTransactions().Should()
                .Contain(transaction => transaction.TransactionType == TransactionType.Deposit)
                .And
                .Contain(transaction => transaction.TransactionType == TransactionType.Transfer);

            account2.GetAccountTransactions().Should()
                .Contain(transaction => transaction.TransactionType == TransactionType.Deposit);
        }

        [Fact]
        public void AccountMadeAPayment_Success()
        {
            var account = new Account("Foo", "foo@gmail.com", "+5551999999999", "123.456.789.10", Currency.BrazilianReal);

            account.Deposit(1000);

            Assert.Equal($"{Currency.BrazilianReal.Symbol}1000", account.GetBalance());

            account.Payment(300);

            Assert.Equal($"{Currency.BrazilianReal.Symbol}700", account.GetBalance());

            account.GetAccountTransactions().Should()
                .Contain(transaction => transaction.TransactionType == TransactionType.Deposit)
                .And
                .Contain(transaction => transaction.TransactionType == TransactionType.Payment);
        }

        [Fact]
        public void AccountWithdraw_Success()
        {
            var account = new Account("Foo", "foo@gmail.com", "+5551999999999", "123.456.789.10", Currency.BrazilianReal);

            account.Deposit(1000);

            Assert.Equal($"{Currency.BrazilianReal.Symbol}1000", account.GetBalance());

            account.Withdraw(1000);

            Assert.Equal($"{Currency.BrazilianReal.Symbol}0", account.GetBalance());

            account.GetAccountTransactions().Should()
                .Contain(transaction => transaction.TransactionType == TransactionType.Deposit)
                .And
                .Contain(transaction => transaction.TransactionType == TransactionType.Withdraw);
        }
    }
}