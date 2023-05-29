using BankingApp.Accounts.Domain.ValueObjects;
using BankingApp.Application.Core.Extensions;
using FluentValidation;

namespace BankingApp.Accounts.API.Features.CreateAccount;

public class CreateAccountCommandValidator : AbstractValidator<CreateAccountCommand>
{
    public CreateAccountCommandValidator()
    {
        RuleFor(command => command.FirstName)
            .NotNull()
            .NotEmpty();

        RuleFor(command => command.LastName)
            .NotNull()
            .NotEmpty();

        RuleFor(command => command.Document)
            .NotNull()
            .NotEmpty();

        RuleFor(command => command.Currency)
            .NotNull()
            .NotEmpty()
            .MustBeOneOf(Currency.Enumerate<Currency>().Select(currency => currency.Value));
    }
}