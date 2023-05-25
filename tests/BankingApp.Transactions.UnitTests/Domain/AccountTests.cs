namespace BankingApp.Transactions.UnitTests.Domain;

public class AccountTests
{
    [Theory]
    [InlineData(null, "99999999999", "john.doe@gmail.com", "+555199999999", "99999999999")]
    [InlineData("", "99999999999", "john.doe@gmail.com", "+555199999999", "99999999999")]
    [InlineData("John Doe", null, "john.doe@gmail.com", "+555199999999", "99999999999")]
    [InlineData("John Doe", "", "john.doe@gmail.com", "+555199999999", "99999999999")]
    [InlineData("John Doe", "99999999999", null, "+555199999999", "99999999999")]
    [InlineData("John Doe", "99999999999", "", "+555199999999", "99999999999")]
    [InlineData("John Doe", "99999999999", "john.doe@gmail.com", null, "99999999999")]
    [InlineData("John Doe", "99999999999", "john.doe@gmail.com", "", "99999999999")]
    [InlineData("John Doe", "99999999999", "john.doe@gmail.com", "+555199999999", null)]
    [InlineData("John Doe", "99999999999", "john.doe@gmail.com", "+555199999999", "")]
    public void Account_WhenConstructorParametersAreNullOrEmpty_ShouldThrowArgumentNullException(
        string name,
        string document,
        string email,
        string phoneNumber,
        string token)
    {
        // Arrange
        Account CreateAccount() => new(name, document, email, phoneNumber, token, Currency.Dollar);

        // Act & Assert
        Assert.Throws<ArgumentNullException>(CreateAccount);
    }

    [Fact]
    public void Account_GetFormattedCurrentBalance_ShouldReturnBalanceCurrencyWithSymbol()
    {
        // Arrange
        var account = new Account("John Doe",
            "99999999999",
            "john.doe@gmail.com",
            "+555199999999",
            "99999999999",
            Currency.Dollar
        );

        // Act
        var balance = account.GetFormattedCurrentBalance();

        // Assert
        balance.Should().Be("$0.00");
    }

    [Fact]
    public void Account_GetCurrentBalance_ShouldReturnBalanceAsDecimal()
    {
        // Arrange
        var account = new Account("John Doe",
            "99999999999",
            "john.doe@gmail.com",
            "+555199999999",
            "99999999999",
            Currency.Dollar
        );

        // Act
        var balance = account.GetCurrentBalance();

        // Assert
        balance.Should().Be(decimal.Zero);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-1000)]
    public void Account_DepositWhenAmountIsLessThanOrEqualToZero_ShouldThrowInvalidTransactionValueException(decimal amount)
    {
        // Arrange
        var account = new Account("John Doe",
            "99999999999",
            "john.doe@gmail.com",
            "+555199999999",
            "99999999999",
            Currency.Dollar
        );

        // Act & Assert
        Assert.Throws<InvalidTransactionValueException>(() => account.Deposit(amount, Currency.Euro, DateTime.Now));
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-1000)]
    public void Account_WithdrawWhenAmountIsLessThanOrEqualToZero_ShouldThrowInvalidTransactionValueException(decimal amount)
    {
        // Arrange
        var account = new Account("John Doe",
            "99999999999",
            "john.doe@gmail.com",
            "+555199999999",
            "99999999999",
            Currency.Dollar
        );

        // Act & Assert
        Assert.Throws<InvalidTransactionValueException>(() => account.Withdraw(amount, DateTime.Now));
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-1000)]
    public void Account_TransferWhenAmountIsLessThanOrEqualToZero_ShouldThrowInvalidTransactionValueException(decimal amount)
    {
        // Arrange
        var account = new Account("John Doe",
            "99999999999",
            "john.doe@gmail.com",
            "+555199999999",
            "99999999999",
            Currency.Dollar
        );

        // Act & Assert
        Assert.Throws<InvalidTransactionValueException>(() => account.Transfer(amount, DateTime.Now));
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-1000)]
    public void Account_ApplyEarningsWhenValueIsLessThanOrEqualToZero_ShouldThrowInvalidTransactionValueException(decimal earnings)
    {
        // Arrange
        var account = new Account("John Doe",
            "99999999999",
            "john.doe@gmail.com",
            "+555199999999",
            "99999999999",
            Currency.Dollar
        );

        // Act & Assert
        Assert.Throws<InvalidTransactionValueException>(() => account.ApplyEarnings(earnings, DateTime.Now));
    }

    [Fact]
    public void Account_DepositWhenAmountIsValid_ShouldIncreaseBalance()
    {
        // Arrange
        var depositAmount = new Money(1000.00m);
        var account = new Account("John Doe",
            "99999999999",
            "john.doe@gmail.com",
            "+555199999999",
            "99999999999",
            Currency.Dollar
        );

        // Act
        account.Deposit(depositAmount, Currency.Euro, DateTime.Now);
        var balance = account.GetCurrentBalance();

        // Assert
        var expected = new Money(depositAmount / Currency.Euro.DollarRate);
        balance.Should().Be(expected.Value);
    }
    [Fact]
    public void Account_WithdrawWhenAmountIsValid_ShouldDecreaseBalance()
    {
        // Arrange
        var depositAmount = new Money(1000.00m);
        var withdrawAmount = new Money(500.00m);
        var account = new Account("John Doe",
            "99999999999",
            "john.doe@gmail.com",
            "+555199999999",
            "99999999999",
            Currency.Dollar
        );

        // Act
        account.Deposit(depositAmount, Currency.BrazilianReal, DateTime.Now);
        account.Withdraw(withdrawAmount, DateTime.Now);

        var balance = account.GetCurrentBalance();

        // Assert
        var expected = new Money(depositAmount / Currency.BrazilianReal.DollarRate - withdrawAmount);
        balance.Should().Be(expected.Value);
    }

    [Fact]
    public void Account_TransferWhenAmountIsValid_ShouldDecreaseBalance()
    {
        // Arrange
        var depositAmount = new Money(1000.00m);
        var transferAmount = new Money(500.00m);
        var account = new Account("John Doe",
            "99999999999",
            "john.doe@gmail.com",
            "+555199999999",
            "99999999999",
            Currency.Dollar
        );

        // Act
        account.Deposit(depositAmount, Currency.UruguayanPeso, DateTime.Now);
        account.Transfer(transferAmount, DateTime.Now);

        var balance = account.GetCurrentBalance();

        // Assert
        var expected = new Money(depositAmount / Currency.UruguayanPeso.DollarRate - transferAmount);
        balance.Should().Be(expected.Value);
    }

    [Fact]
    public void Account_ApplyEarningsWhenAmountIsValid_ShouldDecreaseBalance()
    {
        // Arrange
        var depositAmount = new Money(1000.00m);
        var earnings = new Money(1.01m);
        var account = new Account("John Doe",
            "99999999999",
            "john.doe@gmail.com",
            "+555199999999",
            "99999999999",
            Currency.Dollar
        );

        // Act
        account.Deposit(depositAmount, Currency.BritishPound, DateTime.Now);
        account.ApplyEarnings(earnings, DateTime.Now);

        var balance = account.GetCurrentBalance();

        // Assert
        var expected = new Money(depositAmount / Currency.BritishPound.DollarRate * (earnings * Currency.Dollar.DollarRate));
        balance.Should().Be(expected.Value);
    }

    [Fact]
    public void Account_GetTransactionsByPeriod_ShouldReturnAllTransactions()
    {
        // Arrange
        var start = new DateTime(2023, 5, 20);
        var end = new DateTime(2023, 5, 25);
        var account = new Account("John Doe",
            "99999999999",
            "john.doe@gmail.com",
            "+555199999999",
            "99999999999",
            Currency.Dollar
        );
        account.Deposit(1000.00m, Currency.Dollar, new DateTime(2023, 5, 20, 11, 0, 0));
        account.Deposit(500.00m, Currency.Dollar, new DateTime(2023, 5, 23, 14, 12, 0));
        account.Deposit(2000.00m, Currency.Dollar, new DateTime(2023, 5, 25, 14, 54, 0));
        account.Deposit(2000.00m, Currency.Dollar, new DateTime(2023, 5, 27, 18, 43, 0));

        // Act
        var transactions = account.GetBalanceStatementByPeriod(start, end);

        // Assert
        transactions.Should().NotBeEmpty().And.HaveCount(3);
    }

    [Fact]
    public void Account_ChangeCurrency_ShouldChangeTheDefaultAccountCurrency()
    {
        // Arrange
        var account = new Account("John Doe",
            "99999999999",
            "john.doe@gmail.com",
            "+555199999999",
            "99999999999",
            Currency.Dollar
        );
        var oldCurrency = account.Currency;

        // Act
        account.ChangeCurrency(Currency.BritishPound);

        // Assert
        oldCurrency.Should().Be(Currency.Dollar);
        account.Currency.Should().Be(Currency.BritishPound);
    }
}