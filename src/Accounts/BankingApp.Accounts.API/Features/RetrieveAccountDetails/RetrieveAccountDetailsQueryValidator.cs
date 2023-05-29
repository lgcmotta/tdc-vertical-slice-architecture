using FluentValidation;

namespace BankingApp.Accounts.API.Features.RetrieveAccountDetails;

public class RetrieveAccountDetailsQueryValidator : AbstractValidator<RetrieveAccountDetailsQuery>
{
    public RetrieveAccountDetailsQueryValidator()
    {
        RuleFor(query => query.Token)
            .NotNull()
            .NotEmpty();
    }
}