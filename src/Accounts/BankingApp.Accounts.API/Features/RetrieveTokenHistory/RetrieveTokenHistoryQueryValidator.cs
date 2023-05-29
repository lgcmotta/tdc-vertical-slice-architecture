using FluentValidation;

namespace BankingApp.Accounts.API.Features.RetrieveTokenHistory;

public class RetrieveTokenHistoryQueryValidator : AbstractValidator<RetrieveTokenHistoryQuery>
{
    public RetrieveTokenHistoryQueryValidator()
    {
        RuleFor(query => query.Token)
            .NotNull()
            .NotEmpty();
    }
}