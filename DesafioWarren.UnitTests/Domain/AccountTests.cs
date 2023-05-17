using DesafioWarren.Domain.Aggregates;
using DesafioWarren.Domain.ValueObjects;
using Xunit;

namespace DesafioWarren.UnitTests.Domain
{
    public class AccountTests
    {
        [Fact]
        public void CreateNewAccount_Sucess()
        {
            var account = new Account("Foo", "foo@gmail.com", "+5551999999999", "123.456.789.10", Currency.BrazilianReal);

            Assert.NotNull(account);
        }

        [Fact]
        public void GetAccountName_Sucess()
        {
            var account = new Account("Foo", "foo@gmail.com", "+5551999999999", "123.456.789.10", Currency.BrazilianReal);

            Assert.Equal("Foo", account.Name);
        }

        [Fact]
        public void GetAccountEmail_Sucess()
        {
            var account = new Account("Foo", "foo@gmail.com", "+5551999999999", "123.456.789.10", Currency.BrazilianReal);

            Assert.Equal("foo@gmail.com", account.Email);
        }

        [Fact]
        public void GetAccountPhoneNumber_Sucess()
        {
            var account = new Account("Foo", "foo@gmail.com", "+5551999999999", "123.456.789.10", Currency.BrazilianReal);

            Assert.Equal("+5551999999999", account.PhoneNumber);
        }

        [Fact]
        public void ChangeAccountEmail_Succes()
        {
            var account = new Account("Foo", "foo@gmail.com", "+5551999999999", "123.456.789.10", Currency.BrazilianReal);

            account.ChangeEmail("foo2@gmail.com");

            Assert.Equal("foo2@gmail.com", account.Email);

            Assert.NotEqual("foo@gmail.com", account.Email);
        }

        [Fact]
        public void ChangePhoneNumber_Sucess()
        {
            var account = new Account("Foo", "foo@gmail.com", "+5551999999999", "123.456.789.10", Currency.BrazilianReal);

            account.ChangePhoneNumber("+5551888888888");

            Assert.Equal("+5551888888888", account.PhoneNumber);

            Assert.NotEqual("+5551999999999", account.PhoneNumber);
        }
    }
}