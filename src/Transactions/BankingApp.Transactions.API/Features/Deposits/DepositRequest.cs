// ReSharper disable ClassNeverInstantiated.Global
namespace BankingApp.Transactions.API.Features.Deposits;

public record DepositRequest(string Token, string Currency, decimal Amount);