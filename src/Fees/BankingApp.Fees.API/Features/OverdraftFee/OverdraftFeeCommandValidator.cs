using FluentValidation;

namespace BankingApp.Fees.API.Features.OverdraftFee;

public class OverdraftFeeCommandValidator : AbstractValidator<OverdraftFeeCommand>
{
    public OverdraftFeeCommandValidator()
    {
        RuleFor(command => command.Rate)
            .GreaterThan(0.0m);
    }
}