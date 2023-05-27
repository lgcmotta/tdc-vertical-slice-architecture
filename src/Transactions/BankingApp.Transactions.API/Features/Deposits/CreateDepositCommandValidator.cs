using BankingApp.Application.Core.Extensions;
using BankingApp.Transactions.Domain.ValueObjects;
using FluentValidation;

namespace BankingApp.Transactions.API.Features.Deposits;

public class CreateDepositCommandValidator : AbstractValidator<CreateDepositCommand>
{
    public CreateDepositCommandValidator()
    {
        RuleFor(command => command.Token)
            .NotNull()
            .NotEmpty();

        RuleFor(command => command.Amount)
            .GreaterThan(decimal.Zero);

        RuleFor(command => command.Currency)
            .NotEmpty()
            .NotEmpty()
            .MustBeOneOf(Currency.Enumerate<Currency>().Select(currency => currency.Value));
    }
}