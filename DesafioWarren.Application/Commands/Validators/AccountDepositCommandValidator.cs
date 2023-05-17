using DesafioWarren.Domain.Repositories;
using FluentValidation;

namespace DesafioWarren.Application.Commands.Validators
{
    public class AccountDepositCommandValidator : AbstractValidator<AccountDepositCommand>
    {
        public AccountDepositCommandValidator(IAccountRepository accountRepository)
        {
            RuleFor(command => command.Value)
                .GreaterThan(0);

            RuleFor(command => command.AccountId)
                .MustAsync(async (accountId, cancellationToken) =>
                {
                    var account = await accountRepository.GetAccountByIdAsync(accountId, cancellationToken);

                    return account is not null;
                })
                .WithMessage("There's no account with the provided id.");
        }
    }
}