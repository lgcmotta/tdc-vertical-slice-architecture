using FluentValidation;

namespace BankingApp.Transactions.API.Features.ApplyOverdraftFee;

public class ApplyOverdraftFeeCommandValidator : AbstractValidator<ApplyOverdraftFeeCommand>
{
    public ApplyOverdraftFeeCommandValidator()
    {
        RuleFor(command => command.HolderId)
            .Must(holderId => holderId != Guid.Empty);
    }
}