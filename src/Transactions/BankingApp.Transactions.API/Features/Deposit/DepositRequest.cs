// ReSharper disable ClassNeverInstantiated.Global
namespace BankingApp.Transactions.API.Features.Deposit;

public record DepositRequest(string Token, string Currency, decimal Amount);