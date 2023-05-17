using System.Linq;
using DesafioWarren.Domain.Repositories;
using DesafioWarren.Domain.ValueObjects;
using FluentValidation;

namespace DesafioWarren.Application.Commands.Validators
{
    public class CreateAccountCommandValidator : AbstractValidator<CreateAccountCommand>
    {
        public CreateAccountCommandValidator(IAccountRepository accountRepository)
        {
            RuleFor(command => command.Account.Name)
                .PropertyMustNotBeNullOrEmpty();

            RuleFor(command => command.Account.Cpf)
                .PropertyMustNotBeNullOrEmpty();

            RuleFor(command => command.Account.PhoneNumber)
                .PropertyMustNotBeNullOrEmpty();

            RuleFor(command => command.Account.Email)
                .PropertyMustNotBeNullOrEmpty();

            RuleFor(command => command.Account.Currency)
                .Must(currency => Enumeration.GetEnumerationItems<Currency>()
                    .Select(possibleCurrency => possibleCurrency.Value)
                    .Contains(currency));

            RuleFor(command => command.Account.Name)
                .MustAsync(async (name, cancellationToken) =>
                {
                    var account = await accountRepository.GetAccountByNameAsync(name, cancellationToken);

                    return account is null;
                })
                .WithMessage("There's already an account that belong to this person.");

            RuleFor(command => command.Account.Cpf)
                .MustAsync(async (cpf, cancellationToken) =>
                {
                    var account = await accountRepository.GetAccountByCpf(cpf, cancellationToken);

                    return account is null;

                }).WithMessage("This CPF already has an account.");
        }
        
        
    }
}