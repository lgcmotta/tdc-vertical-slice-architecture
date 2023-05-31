using BankingApp.Fees.API.Infrastructure;
using BankingApp.Taxes.Domain.Exceptions;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace BankingApp.Fees.API.Features.UpdateAccount;

public class UpdateAccountCommandHandler : IRequestHandler<UpdateAccountCommand>
{
    private readonly AccountFeesDbContext _context;

    public UpdateAccountCommandHandler(AccountFeesDbContext context)
    {
        _context = context;
    }

    public async Task Handle(UpdateAccountCommand request, CancellationToken cancellationToken)
    {
        var account = await _context.Accounts
            .FirstOrDefaultAsync(account => account.Id == request.HolderId, cancellationToken)
            .ConfigureAwait(continueOnCapturedContext: false);

        if (account is null)
        {
            throw new AccountNotFoundException($"Account not found for account holder {request.HolderId}");
        }

        if (!string.IsNullOrWhiteSpace(request.Token))
        {
            account.Token = request.Token;
        }
    }
}