using BankingApp.Application.Core.Extensions;
using BankingApp.Transactions.Domain.ValueObjects;
using FluentValidation;

namespace BankingApp.Transactions.API.Features.AccountUpdated;

public class UpdateAccountCommandValidator : AbstractValidator<UpdateAccountCommand>
{
    public UpdateAccountCommandValidator()
    {
        RuleFor(command => command.HolderId)
            .Must(holderId => holderId != Guid.Empty)
            .WithMessage($"{{PropertyName}} must not be an empty Guid.");

        When(command => command.Currency is not null, () =>
        {
            RuleFor(command => command.Currency)
                .NotEmpty()
                .MustBeOneOf(Currency.Enumerate<Currency>().Select(currency => currency.Value));
        });

        When(command => command.Name is not null, () =>
        {
            RuleFor(command => command.Name)
                .NotEmpty();
        });

        When(command => command.Token is not null, () =>
        {
            RuleFor(command => command.Token)
                .NotEmpty();
        });
    }
}