using BankingApp.Application.Core.Extensions;
using BankingApp.Transactions.Domain.ValueObjects;
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

        RuleFor(command => command.Currency)
            .NotNull()
            .NotEmpty()
            .MustBeOneOf(Currency.Enumerate<Currency>().Select(currency => currency.Value));
    }
}