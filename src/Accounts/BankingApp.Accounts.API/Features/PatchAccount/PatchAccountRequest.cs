namespace BankingApp.Accounts.API.Features.PatchAccount;

public record PatchAccountRequest(string Token, string? FirstName, string? LastName, string? Document, string? Currency);