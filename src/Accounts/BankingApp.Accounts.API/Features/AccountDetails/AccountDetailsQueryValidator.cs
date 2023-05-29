using FluentValidation;

namespace BankingApp.Accounts.API.Features.AccountDetails;

public class AccountDetailsQueryValidator : AbstractValidator<AccountDetailsQuery>
{
    public AccountDetailsQueryValidator()
    {
        RuleFor(query => query.Token)
            .NotNull()
            .NotEmpty();
    }
}