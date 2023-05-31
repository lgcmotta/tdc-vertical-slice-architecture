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
    public void Account_WhenConstructorStringArgumentsAreNullEmptyOrWhiteSpace_ShouldThrowArgumentNullException(Guid holderId, string name, string document, string token)
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
    public void Account_TransferOutWhenAmountIsLessThanOrEqualToZero_ShouldThrowInvalidTransactionValueException(Money amount)
    {
        // Arrange
        var sender = _fixture.GenerateStandardAccount();
        var receiver = _fixture.GenerateStandardAccount();

        var currency = _fixture.PickRandomCurrency();

        // Act & Assert
        Assert.Throws<InvalidTransactionValueException>(() => sender.TransferOut(receiver.Id, amount, currency, DateTime.Now));
    }

    [Theory]
    [ClassData(typeof(AccountFixture.InvalidTransactionAmounts))]
    public void Account_TransferInWhenAmountIsLessThanOrEqualToZero_ShouldThrowInvalidTransactionValueException(Money amount)
    {
        // Arrange
        var sender = _fixture.GenerateStandardAccount();
        var receiver = _fixture.GenerateStandardAccount();

        var currency = _fixture.PickRandomCurrency();

        // Act & Assert
        Assert.Throws<InvalidTransactionValueException>(() => receiver.TransferIn(sender.Id, amount, currency, DateTime.Now));
    }

    [Fact]
    public void Account_DepositWhenAmountIsValid_ShouldIncreaseBalance()
    {
        // Arrange
        var depositAmount = _fixture.GenerateMoney();
        var currency = _fixture.PickRandomCurrency();
        var account = _fixture.GenerateStandardAccount();

        // Act
        account.Deposit(depositAmount, currency, DateTime.Now);

        // Assert
        var expectedBalance = depositAmount / currency.DollarExchangeRate;
        account.GetCurrentBalance().Should().Be(expectedBalance.Value);
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

        // Assert
        var expectedBalance = depositAmount / currency.DollarExchangeRate - withdrawAmount;
        account.GetCurrentBalance().Should().Be(expectedBalance.Value);
    }

    [Fact]
    public void Account_TransferWhenAmountIsValid_ShouldDecreaseBalance()
    {
        // Arrange
        var sender = _fixture.GenerateStandardAccount();
        var receiver = _fixture.GenerateStandardAccount();

        var senderDeposit = _fixture.GenerateMoney();
        var receiverDeposit = _fixture.GenerateMoney();
        var transferAmount = _fixture.GenerateMoneyBetween(senderDeposit, receiverDeposit);

        var currency = _fixture.PickRandomCurrency();

        sender.Deposit(senderDeposit, currency, DateTime.Now);
        receiver.Deposit(receiverDeposit, currency, DateTime.Now);

        // Act
        sender.TransferOut(receiver.Id, transferAmount, currency, DateTime.Now);
        receiver.TransferIn(sender.Id, transferAmount, currency, DateTime.Now);

        var senderExpectedBalance = Money.ConvertToUSD(senderDeposit, currency) - Money.ConvertToUSD(transferAmount, currency);
        var receiverExpectedBalance = Money.ConvertToUSD(receiverDeposit, currency) + Money.ConvertToUSD(transferAmount, currency);

        // Assert
        sender.GetCurrentBalance().Should().Be(senderExpectedBalance.Value);
        receiver.GetCurrentBalance().Should().Be(receiverExpectedBalance.Value);
    }

    [Fact]
    public void Account_ApplyProfitFee_ShouldIncreaseBalance()
    {
        // Arrange
        var account = _fixture.GenerateStandardAccount();

        // Act
        account.Deposit(new Money(100.00m), Currency.Dollar, DateTime.Now);
        account.ApplyProfitFee(new Money(1.0m), DateTime.Now);

        var balance = account.GetCurrentBalance();

        // Assert
        var expected = new Money(101.00m);
        balance.Should().Be(expected.Value);
    }

    [Fact]
    public void Account_ApplyOverdraftFee_ShouldDecreaseBalance()
    {
        // Arrange
        var account = _fixture.GenerateStandardAccount();

        // Act
        account.Deposit(new Money(100.00m), Currency.Dollar, DateTime.Now);
        account.Withdraw(new Money(200.00m), DateTime.Now);
        account.ApplyProfitFee(new Money(-10.00m), DateTime.Now);

        var balance = account.GetCurrentBalance();

        // Assert
        var expected = new Money(-110.00m);
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
        var oldCurrency = account.DisplayCurrency;

        // Act
        account.ChangeCurrency(Currency.PoundSterling);

        // Assert
        oldCurrency.Should().Be(Currency.Dollar);
        account.DisplayCurrency.Should().Be(Currency.PoundSterling);
    }
}