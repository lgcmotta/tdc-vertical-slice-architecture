using BankingApp.Application.Core.Extensions;
using FluentValidation;

namespace BankingApp.Transactions.API.Features.RetrieveMonthlyStatement;

public class RetrieveMonthlyStatementQueryValidator : AbstractValidator<RetrieveMonthlyStatementQuery>
{
    public RetrieveMonthlyStatementQueryValidator()
    {
        RuleFor(query => query.Token)
            .NotNull()
            .NotEmpty();

        RuleFor(query => query.Month)
            .MustBeOneOf(Enumerable.Range(1, 12));
    }
}