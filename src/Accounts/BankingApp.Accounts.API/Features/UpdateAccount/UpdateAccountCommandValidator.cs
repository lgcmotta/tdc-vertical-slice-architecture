using BankingApp.Accounts.Domain.ValueObjects;
using BankingApp.Application.Core.Extensions;
using FluentValidation;

namespace BankingApp.Accounts.API.Features.UpdateAccount;

public class UpdateAccountCommandValidator : AbstractValidator<UpdateAccountCommand>
{
    public UpdateAccountCommandValidator()
    {
        RuleFor(command => command.Token)
            .NotNull()
            .NotEmpty();
        
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