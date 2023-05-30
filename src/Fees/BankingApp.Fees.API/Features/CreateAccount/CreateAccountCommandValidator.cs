using FluentValidation;

namespace BankingApp.Fees.API.Features.CreateAccount;

public class CreateAccountCommandValidator : AbstractValidator<CreateAccountCommand>
{
    public CreateAccountCommandValidator()
    {
        RuleFor(command => command.HolderId)
            .NotNull()
            .Must(holderId => holderId != Guid.Empty);

        RuleFor(command => command.Token)
            .NotNull()
            .NotEmpty();
    }
}