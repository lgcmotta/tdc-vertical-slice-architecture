using BankingApp.Application.Core.Extensions;
using BankingApp.Transactions.Domain.ValueObjects;
using FluentValidation;

namespace BankingApp.Transactions.API.Features.CreateAccount;

public class CreateAccountCommandValidator : AbstractValidator<CreateAccountCommand>
{
    public CreateAccountCommandValidator()
    {
        RuleFor(command => command.HolderId)
            .Must(holderId => holderId != Guid.Empty)
            .WithMessage($"{{PropertyName}} must not be an empty Guid.");

        RuleFor(command => command.Name)
            .NotEmpty()
            .NotNull();

        RuleFor(command => command.Document)
            .NotEmpty()
            .NotEmpty();

        RuleFor(command => command.Token)
            .NotEmpty()
            .NotEmpty();

        RuleFor(command => command.Currency)
            .NotEmpty()
            .NotEmpty()
            .MustBeOneOf(Currency.Enumerate<Currency>().Select(currency => currency.Value));
    }
}