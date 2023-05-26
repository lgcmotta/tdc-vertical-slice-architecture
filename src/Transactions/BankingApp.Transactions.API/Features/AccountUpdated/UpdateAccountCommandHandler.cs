using BankingApp.Transactions.API.Infrastructure;
using BankingApp.Transactions.Domain.Exceptions;
using BankingApp.Transactions.Domain.ValueObjects;
using MediatR;
using Microsoft.EntityFrameworkCore;

// ReSharper disable once ClassNeverInstantiated.Global
namespace BankingApp.Transactions.API.Features.AccountUpdated;

public record UpdateAccountCommand(Guid HolderId, string? Name, string? Document, string? Token, string? Currency) : IRequest;

public class UpdateAccountCommandHandler : IRequestHandler<UpdateAccountCommand>
{
    private readonly AccountsDbContext _context;

    public UpdateAccountCommandHandler(AccountsDbContext context)
    {
        _context = context;
    }

    public async Task Handle(UpdateAccountCommand request, CancellationToken cancellationToken)
    {
        var account = await _context.Accounts.FirstOrDefaultAsync(account => account.Holder.Id == request.HolderId, cancellationToken)
            .ConfigureAwait(continueOnCapturedContext: false);

        if (account is null)
        {
            throw new AccountNotFoundException($"Account not found for account holder {request.HolderId}");
        }

        account.ChangeHolderName(request.Name);
        account.ChangeHolderDocument(request.Document);
        account.UpdateHolderToken(request.Token);

        if (!string.IsNullOrWhiteSpace(request.Currency) && Currency.TryParseByValue<Currency>(request.Currency, out var currency))
        {
            account.ChangeCurrency(currency!);
        }
    }
}