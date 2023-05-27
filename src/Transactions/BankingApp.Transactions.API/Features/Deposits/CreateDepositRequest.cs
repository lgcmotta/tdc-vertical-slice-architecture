// ReSharper disable ClassNeverInstantiated.Global
namespace BankingApp.Transactions.API.Features.Deposits;

public record CreateDepositRequest(string Token, string Currency, decimal Amount);