using FluentValidation;

namespace BankingApp.Transactions.API.Features.YearlyStatement;

public class YearlyStatementQueryValidator : AbstractValidator<YearlyStatementQuery>
{
    public YearlyStatementQueryValidator()
    {
        RuleFor(query => query.Token)
            .NotNull()
            .NotEmpty();

        RuleFor(query => query.Year)
            .GreaterThan(2022)
            .LessThanOrEqualTo(DateTime.UtcNow.Year);
    }
}