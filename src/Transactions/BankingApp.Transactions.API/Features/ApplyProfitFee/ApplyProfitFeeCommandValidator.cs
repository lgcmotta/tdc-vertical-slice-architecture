using FluentValidation;

namespace BankingApp.Transactions.API.Features.ApplyProfitFee;

public class ApplyProfitFeeCommandValidator : AbstractValidator<ApplyProfitFeeCommand>
{
    public ApplyProfitFeeCommandValidator()
    {
        RuleFor(command => command.HolderId)
            .Must(holderId => holderId != Guid.Empty);
    }
}