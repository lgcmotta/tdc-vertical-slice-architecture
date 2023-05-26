namespace BankingApp.Transactions.UnitTests.Domain;

public class AccountTests : IClassFixture<AccountFixture>
{
    private readonly AccountFixture _fixture;

    public AccountTests(AccountFixture fixture)
    {
        _fixture = fixture;
    }

    [Theory]
    [ClassData(typeof(AccountFixture.InvalidAccountConstructorParams))]
    public void Account_WhenTokenIsNullOrEmpty_ShouldThrowArgumentNullException(Guid holderId, string name, string document, string token)
    {
        // Arrange
        Account CreateAccount() => new(holderId, name, document, token, Currency.Dollar);

        // Act & Assert
        Assert.Throws<ArgumentNullException>(CreateAccount);
    }

    [Fact]
    public void Account_GetFormattedCurrentBalance_ShouldReturnBalanceCurrencyWithSymbol()
    {
        // Arrange
        var account = _fixture.GenerateStandardAccount();

        // Act
        var balance = account.GetFormattedCurrentBalance();

        // Assert
        balance.Should().Be("$0.00");
    }

    [Fact]
    public void Account_GetCurrentBalance_ShouldReturnBalanceAsDecimal()
    {
        // Arrange
        var account = _fixture.GenerateStandardAccount();

        // Act
        var balance = account.GetCurrentBalance();

        // Assert
        balance.Should().Be(decimal.Zero);
    }

    [Theory]
    [ClassData(typeof(AccountFixture.InvalidTransactionAmounts))]
    public void Account_DepositWhenAmountIsLessThanOrEqualToZero_ShouldThrowInvalidTransactionValueException(Money amount)
    {
        // Arrange
        var account = _fixture.GenerateStandardAccount();

        // Act & Assert
        Assert.Throws<InvalidTransactionValueException>(() => account.Deposit(amount, Currency.Euro, DateTime.Now));
    }

    [Theory]
    [ClassData(typeof(AccountFixture.InvalidTransactionAmounts))]
    public void Account_WithdrawWhenAmountIsLessThanOrEqualToZero_ShouldThrowInvalidTransactionValueException(Money amount)
    {
        // Arrange
        var account = _fixture.GenerateStandardAccount();

        // Act & Assert
        Assert.Throws<InvalidTransactionValueException>(() => account.Withdraw(amount, DateTime.Now));
    }

    [Theory]
    [ClassData(typeof(AccountFixture.InvalidTransactionAmounts))]
    public void Account_TransferWhenAmountIsLessThanOrEqualToZero_ShouldThrowInvalidTransactionValueException(Money amount)
    {
        // Arrange
        var sender = _fixture.GenerateStandardAccount();
        var receiver = _fixture.GenerateStandardAccount();

        // Act & Assert
        Assert.Throws<InvalidTransactionValueException>(() => sender.Transfer(amount, receiver, DateTime.Now));
    }

    [Theory]
    [ClassData(typeof(AccountFixture.InvalidTransactionAmounts))]
    public void Account_ApplyEarningsWhenValueIsLessThanOrEqualToZero_ShouldThrowInvalidTransactionValueException(Money earnings)
    {
        // Arrange
        var account = _fixture.GenerateStandardAccount();

        // Act & Assert
        Assert.Throws<InvalidTransactionValueException>(() => account.ApplyEarnings(earnings, DateTime.Now));
    }

    [Fact]
    public void Account_DepositWhenAmountIsValid_ShouldIncreaseBalance()
    {
        // Arrange
        var amount = _fixture.GenerateMoney();
        var currency = _fixture.PickRandomCurrency();
        var account = _fixture.GenerateStandardAccount();

        // Act
        account.Deposit(amount, currency, DateTime.Now);
        var balance = account.GetCurrentBalance();

        // Assert
        var expected = new Money(amount / currency.DollarRate);
        balance.Should().Be(expected.Value);
    }
    [Fact]
    public void Account_WithdrawWhenAmountIsValid_ShouldDecreaseBalance()
    {
        // Arrange
        var depositAmount = _fixture.GenerateMoney();
        var withdrawAmount = _fixture.GenerateMoney();
        var currency = _fixture.PickRandomCurrency();
        var account = _fixture.GenerateStandardAccount();

        // Act
        account.Deposit(depositAmount, currency, DateTime.Now);
        account.Withdraw(withdrawAmount, DateTime.Now);

        var balance = account.GetCurrentBalance();

        // Assert
        var expected = new Money(depositAmount / currency.DollarRate - withdrawAmount);
        balance.Should().Be(expected.Value);
    }

    [Fact]
    public void Account_TransferWhenAmountIsValid_ShouldDecreaseBalance()
    {
        // Arrange
        var senderDeposit = _fixture.GenerateMoney();
        var receiverDeposit = _fixture.GenerateMoney();
        var transferAmount = _fixture.GenerateMoneyBetween(senderDeposit, receiverDeposit);

        var senderDepositCurrency = _fixture.PickRandomCurrency();
        var receiverDepositCurrency = _fixture.PickRandomCurrency();

        var sender = _fixture.GenerateStandardAccount();
        var receiver = _fixture.GenerateStandardAccount();

        // Act
        sender.Deposit(senderDeposit, senderDepositCurrency, DateTime.Now);
        receiver.Deposit(receiverDeposit, receiverDepositCurrency, DateTime.Now);
        sender.Transfer(transferAmount, receiver, DateTime.Now);

        var senderBalance = sender.GetCurrentBalance();
        var receiverBalance = receiver.GetCurrentBalance();

        // Assert
        var senderExpected = new Money(senderDeposit / senderDepositCurrency.DollarRate - transferAmount);
        var receiverExpected = new Money(receiverDeposit / receiverDepositCurrency.DollarRate + transferAmount);
        senderBalance.Should().Be(senderExpected.Value);
        receiverBalance.Should().Be(receiverExpected.Value);
    }

    [Fact]
    public void Account_ApplyEarningsWhenAmountIsValid_ShouldDecreaseBalance()
    {
        // Arrange
        var depositAmount = _fixture.GenerateMoney();
        var earnings = _fixture.GenerateEarnings();
        var currency = _fixture.PickRandomCurrency();
        var account = _fixture.GenerateStandardAccount();

        // Act
        account.Deposit(depositAmount, currency, DateTime.Now);
        account.ApplyEarnings(earnings, DateTime.Now);

        var balance = account.GetCurrentBalance();

        // Assert
        var expected = new Money(depositAmount / currency.DollarRate + (earnings * Currency.Dollar.DollarRate));
        balance.Should().Be(expected.Value);
    }

    [Fact]
    public void Account_GetTransactionsByPeriod_ShouldReturnAllTransactions()
    {
        // Arrange
        var start = new DateTime(2023, 5, 20);
        var end = new DateTime(2023, 5, 25);
        var account = _fixture.GenerateStandardAccount();
        var deposits = _fixture.GenerateDeposits(start, end);

        foreach (var deposit in deposits)
        {
            account.Deposit(deposit.Amount, deposit.Currency, deposit.Occurrence);
        }

        var amount = _fixture.GenerateMoney();
        var currency = _fixture.PickRandomCurrency();
        account.Deposit(amount, currency, new DateTime(2023, 5, 31));

        // Act
        var transactions = account.GetBalanceStatementByPeriod(start, end);

        // Assert
        transactions.Should().NotBeEmpty().And.HaveCount(3);
    }

    [Fact]
    public void Account_ChangeCurrency_ShouldChangeTheDefaultAccountCurrency()
    {
        // Arrange
        var account = _fixture.GenerateStandardAccount();
        var oldCurrency = account.Currency;

        // Act
        account.ChangeCurrency(Currency.BritishPound);

        // Assert
        oldCurrency.Should().Be(Currency.Dollar);
        account.Currency.Should().Be(Currency.BritishPound);
    }
}