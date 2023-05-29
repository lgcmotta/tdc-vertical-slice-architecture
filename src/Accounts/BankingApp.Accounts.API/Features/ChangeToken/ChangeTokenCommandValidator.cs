using FluentValidation;

namespace BankingApp.Accounts.API.Features.ChangeToken;

public class ChangeTokenCommandValidator : AbstractValidator<ChangeTokenCommand>
{
    public ChangeTokenCommandValidator()
    {
        RuleFor(command => command.Token)
            .NotNull()
            .NotEmpty();
    }
}