using FluentValidation;

namespace BankingApp.Transactions.API.Features.RetrieveYearlyStatement;

public class RetrieveYearlyStatementQueryValidator : AbstractValidator<RetrieveYearlyStatementQuery>
{
    public RetrieveYearlyStatementQueryValidator()
    {
        RuleFor(query => query.Token)
            .NotNull()
            .NotEmpty();

        RuleFor(query => query.Year)
            .GreaterThan(2022)
            .LessThanOrEqualTo(DateTime.UtcNow.Year);
    }
}