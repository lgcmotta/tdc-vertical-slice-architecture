using FluentValidation;

namespace BankingApp.Transactions.API.Features.Withdraws;

public class CreateWithdrawCommandValidator : AbstractValidator<CreateWithdrawCommand>
{
    public CreateWithdrawCommandValidator()
    {
        RuleFor(command => command.Token)
            .NotNull()
            .NotEmpty();

        RuleFor(command => command.Amount)
            .GreaterThan(decimal.Zero);
    }
}