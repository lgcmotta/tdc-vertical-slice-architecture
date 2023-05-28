// ReSharper disable ClassNeverInstantiated.Global
namespace BankingApp.Transactions.API.Features.Withdraws;

public record WithdrawRequest(string Token, string Currency, decimal Amount);