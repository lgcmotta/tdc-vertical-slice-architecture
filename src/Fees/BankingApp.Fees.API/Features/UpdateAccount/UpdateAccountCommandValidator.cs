using FluentValidation;

namespace BankingApp.Fees.API.Features.UpdateAccount;

public class UpdateAccountCommandValidator : AbstractValidator<UpdateAccountCommand>
{
    public UpdateAccountCommandValidator()
    {
        RuleFor(command => command.HolderId)
            .Must(holderId => holderId != Guid.Empty);

        When(command => command.Token is not null, () =>
        {
            RuleFor(command => command.Token)
                .NotEmpty();
        });
    }
}