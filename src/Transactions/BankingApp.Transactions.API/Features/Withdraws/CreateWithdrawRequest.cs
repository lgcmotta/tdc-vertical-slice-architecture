// ReSharper disable ClassNeverInstantiated.Global
namespace BankingApp.Transactions.API.Features.Withdraws;

public record CreateWithdrawRequest(string Token, string Currency, decimal Amount);