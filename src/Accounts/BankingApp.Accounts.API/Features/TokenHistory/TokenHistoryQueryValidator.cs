using FluentValidation;

namespace BankingApp.Accounts.API.Features.TokenHistory;

public class TokenHistoryQueryValidator : AbstractValidator<TokenHistoryQuery>
{
    public TokenHistoryQueryValidator()
    {
        RuleFor(query => query.Token)
            .NotNull()
            .NotEmpty();
    }
}