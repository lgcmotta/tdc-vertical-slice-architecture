using BankingApp.Accounts.Domain.ValueObjects;
using BankingApp.Application.Core.Extensions;
using FluentValidation;

namespace BankingApp.Accounts.API.Features.PatchAccount;

public class PatchAccountCommandValidator : AbstractValidator<PatchAccountCommand>
{
    public PatchAccountCommandValidator()
    {
        RuleFor(command => command.Token)
            .NotNull()
            .NotEmpty();

        When(command => command.FirstName is not null, () =>
        {
            RuleFor(command => command.FirstName)
                .NotEmpty();
        });
        
        When(command => command.LastName is not null, () =>
        {
            RuleFor(command => command.LastName)
                .NotEmpty();
        });
        
        When(command => command.Document is not null, () =>
        {
            RuleFor(command => command.Document)
                .NotEmpty();
        });
        
        When(command => command.Currency is not null, () =>
        {
            RuleFor(command => command.Currency)
                .NotEmpty()
                .MustBeOneOf(Currency.Enumerate<Currency>().Select(currency => currency.Value));
        });
        
    }
}