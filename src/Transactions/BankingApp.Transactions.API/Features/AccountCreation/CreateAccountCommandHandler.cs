using BankingApp.Transactions.API.Infrastructure;
using BankingApp.Transactions.Domain;
using BankingApp.Transactions.Domain.Exceptions;
using BankingApp.Transactions.Domain.ValueObjects;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace BankingApp.Transactions.API.Features.AccountCreation;

public class CreateAccountCommandHandler : IRequestHandler<CreateAccountCommand>
{
    private readonly AccountsDbContext _context;

    public CreateAccountCommandHandler(AccountsDbContext context)
    {
        _context = context;
    }

    public async Task Handle(CreateAccountCommand request, CancellationToken cancellationToken)
    {
        var exists = await _context.Accounts.AnyAsync(account => account.Holder.Id == request.HolderId, cancellationToken)
            .ConfigureAwait(continueOnCapturedContext: false);

        if (exists)
        {
            throw new AccountHolderConflictException($"Account holder {request.HolderId} already has an account.");
        }

        var currency = Currency.ParseByValue<Currency>(request.Currency);

        var account = new Account(request.HolderId, request.Name, request.Document, request.Token, currency);

        await _context.Accounts.AddAsync(account, cancellationToken)
            .ConfigureAwait(continueOnCapturedContext: false);
    }
}