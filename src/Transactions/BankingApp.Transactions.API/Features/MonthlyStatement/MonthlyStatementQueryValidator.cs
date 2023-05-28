using BankingApp.Application.Core.Extensions;
using FluentValidation;

namespace BankingApp.Transactions.API.Features.MonthlyStatement;

public class MonthlyStatementQueryValidator : AbstractValidator<MonthlyStatementQuery>
{
    public MonthlyStatementQueryValidator()
    {
        RuleFor(query => query.Token)
            .NotNull()
            .NotEmpty();

        RuleFor(query => query.Month)
            .MustBeOneOf(Enumerable.Range(1, 12));
    }
}