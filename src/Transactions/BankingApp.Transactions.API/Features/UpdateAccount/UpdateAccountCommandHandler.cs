using BankingApp.Transactions.API.Infrastructure;
using BankingApp.Transactions.Domain.Exceptions;
using BankingApp.Transactions.Domain.ValueObjects;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace BankingApp.Transactions.API.Features.UpdateAccount;

public class UpdateAccountCommandHandler : IRequestHandler<UpdateAccountCommand>
{
    private readonly AccountsDbContext _context;

    public UpdateAccountCommandHandler(AccountsDbContext context)
    {
        _context = context;
    }

    public async Task Handle(UpdateAccountCommand request, CancellationToken cancellationToken)
    {
        var account = await _context.Accounts.FirstOrDefaultAsync(account => account.Id == request.HolderId, cancellationToken)
            .ConfigureAwait(continueOnCapturedContext: false);

        if (account is null)
        {
            throw new AccountNotFoundException($"Account not found for account holder {request.HolderId}");
        }

        account.ChangeHolderName(request.Name);
        account.UpdateHolderToken(request.Token);

        if (!string.IsNullOrWhiteSpace(request.Currency) && Currency.TryParseByValue<Currency>(request.Currency, out var currency))
        {
            account.ChangeCurrency(currency!);
        }
    }
}