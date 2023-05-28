﻿using FluentValidation;

namespace BankingApp.Transactions.API.Features.PeriodStatement;

public class PeriodStatementQueryValidator : AbstractValidator<PeriodStatementQuery>
{
    public PeriodStatementQueryValidator()
    {
        RuleFor(query => query.Token)
            .NotNull()
            .NotEmpty();

        When(query => query.Start is not null && query.End is not null, () =>
        {
            RuleFor(query => query.Start)
                .Must((query, start) => start < query.End)
                .WithMessage("Start date must come before end date.");
        });
    }
}