using BankingApp.Accounts.API.Infrastructure;
using BankingApp.Accounts.Domain.Exceptions;
using BankingApp.Accounts.Domain.ValueObjects;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace BankingApp.Accounts.API.Features.UpdateAccount;

public class UpdateAccountCommandHandler : IRequestHandler<UpdateAccountCommand>
{
    private readonly AccountHoldersDbContext _context;

    public UpdateAccountCommandHandler(AccountHoldersDbContext context)
    {
        _context = context;
    }

    public async Task Handle(UpdateAccountCommand request, CancellationToken cancellationToken)
    {
        var account = await _context.Accounts
            .Include(account => account.Tokens.Where(token => token.Enabled))
            .FirstOrDefaultAsync(account => account.Tokens.Any(token => token.Value == request.Token), cancellationToken)
            .ConfigureAwait(continueOnCapturedContext: false);

        if (account is null)
        {
            throw new AccountNotFoundException($"Account not found for token {request.Token}");
        }

        var currency = Currency.ParseByValue<Currency>(request.Currency);

        account.CorrectFirstName(request.FirstName);
        account.CorrectLastName(request.LastName);
        account.ChangeDocument(request.Document);
        account.ChangeCurrency(currency);

        account.AddAccountUpdatedDomainEvent();
    }
}