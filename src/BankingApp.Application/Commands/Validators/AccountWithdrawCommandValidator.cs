﻿using BankingApp.Domain.Repositories;
using FluentValidation;

namespace BankingApp.Application.Commands.Validators;

public class AccountWithdrawCommandValidator : AbstractValidator<AccountWithdrawCommand>
{
    public AccountWithdrawCommandValidator(IAccountRepository accountRepository)
    {
        RuleFor(command => command.Value)
            .GreaterThan(0);

        RuleFor(command => command)
            .MustAsync(async (command, cancellationToken) =>
            {
                var account = await accountRepository.GetAccountByIdAsync(command.AccountId, cancellationToken);

                return account is not null;

            })
            .WithMessage("There's no account with the provided id.")
            .WithName(command => nameof(command.AccountId))
            .MustAsync(async (command, cancellationToken) =>
            {
                var account = await accountRepository.GetAccountByIdAsync(command.AccountId, cancellationToken);

                decimal.TryParse(account.GetBalance().Replace(account.GetCurrencySymbol(), string.Empty)
                    , out var balance); 
                    
                return balance >= command.Value;
            })
            .WithMessage("You don't have sufficient funds to withdraw this value.");
    }
}