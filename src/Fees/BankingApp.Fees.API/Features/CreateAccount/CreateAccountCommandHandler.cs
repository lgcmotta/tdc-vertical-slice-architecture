using BankingApp.Fees.API.Infrastructure;
using BankingApp.Taxes.Domain;
using BankingApp.Taxes.Domain.Exceptions;
using BankingApp.Taxes.Domain.ValueObjects;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace BankingApp.Fees.API.Features.CreateAccount;

public class CreateAccountCommandHandler : IRequestHandler<CreateAccountCommand>
{
    private readonly AccountFeesDbContext _context;

    public CreateAccountCommandHandler(AccountFeesDbContext context)
    {
        _context = context;
    }

    public async Task Handle(CreateAccountCommand request, CancellationToken cancellationToken)
    {
        var accountExists = await _context.Accounts.AnyAsync(account => account.Id == request.HolderId, cancellationToken)
            .ConfigureAwait(continueOnCapturedContext: false);

        if (accountExists)
        {
            throw new AccountHolderConflictException($"Account holder {request.HolderId} already exists.");
        }

        var account = new Account
        {
            Id = request.HolderId,
            Token = request.Token,
            CurrentBalanceInUSD = Money.Zero,
            LastBalanceChange = DateTime.UtcNow
        };

        await _context.Accounts.AddAsync(account, cancellationToken).ConfigureAwait(continueOnCapturedContext: false);
    }
}