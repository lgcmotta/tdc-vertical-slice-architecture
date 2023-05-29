// ReSharper disable ClassNeverInstantiated.Global
namespace BankingApp.Transactions.API.Features.Withdraw;

public record WithdrawRequest(string Token, string Currency, decimal Amount);