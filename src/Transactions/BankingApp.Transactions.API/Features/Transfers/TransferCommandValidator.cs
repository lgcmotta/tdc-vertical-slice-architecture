using FluentValidation;

namespace BankingApp.Transactions.API.Features.Transfers;

public class TransferCommandValidator : AbstractValidator<TransferCommand>
{
    public TransferCommandValidator()
    {
        RuleFor(command => command.Amount)
            .GreaterThan(decimal.Zero);

        RuleFor(command => command.SenderToken)
            .NotNull()
            .NotEmpty();

        RuleFor(command => command.ReceiverToken)
            .NotNull()
            .NotEmpty();
    }
}