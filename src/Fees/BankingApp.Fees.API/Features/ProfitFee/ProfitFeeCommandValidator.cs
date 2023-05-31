using BankingApp.Application.Core.Extensions;
using FluentValidation;

namespace BankingApp.Fees.API.Features.ProfitFee;

public class ProfitFeeCommandValidator : AbstractValidator<ProfitFeeCommand>
{
    public ProfitFeeCommandValidator()
    {
        RuleFor(command => command.Rate)
            .GreaterThan(decimal.Zero);
    }
}