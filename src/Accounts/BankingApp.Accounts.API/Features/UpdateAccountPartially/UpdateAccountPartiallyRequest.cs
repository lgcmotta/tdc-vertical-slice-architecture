namespace BankingApp.Accounts.API.Features.UpdateAccountPartially;

public record UpdateAccountPartiallyRequest(string Token, string? FirstName, string? LastName, string? Document, string? Currency);