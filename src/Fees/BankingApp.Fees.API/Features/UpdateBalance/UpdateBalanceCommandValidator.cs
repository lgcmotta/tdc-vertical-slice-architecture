using FluentValidation;

namespace BankingApp.Fees.API.Features.UpdateBalance;

public class UpdateBalanceCommandValidator : AbstractValidator<UpdateBalanceCommand>
{
    public UpdateBalanceCommandValidator()
    {
        RuleFor(command => command.HolderId)
            .Must(holderId => holderId != Guid.Empty);
    }
}