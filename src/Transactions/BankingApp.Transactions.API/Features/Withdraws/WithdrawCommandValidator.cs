using FluentValidation;

namespace BankingApp.Transactions.API.Features.Withdraws;

public class WithdrawCommandValidator : AbstractValidator<WithdrawCommand>
{
    public WithdrawCommandValidator()
    {
        RuleFor(command => command.Token)
            .NotNull()
            .NotEmpty();

        RuleFor(command => command.Amount)
            .GreaterThan(decimal.Zero);
    }
}