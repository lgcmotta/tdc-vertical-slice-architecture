using BankingApp.Transactions.API.Infrastructure;
using BankingApp.Transactions.Domain.Exceptions;
using BankingApp.Transactions.Domain.ValueObjects;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace BankingApp.Transactions.API.Features.Deposits;

public class DepositCommandHandler : IRequestHandler<DepositCommand, DepositTransactionResponse>
{
    private readonly AccountsDbContext _context;

    public DepositCommandHandler(AccountsDbContext context)
    {
        _context = context;
    }

    public async Task<DepositTransactionResponse> Handle(DepositCommand request, CancellationToken cancellationToken)
    {
        var account = await _context.Accounts
            .FirstOrDefaultAsync(account => account.Holder.Token == request.Token, cancellationToken)
            .ConfigureAwait(continueOnCapturedContext: false);

        if (account is null)
        {
            throw new AccountNotFoundException($"Account not found for token {request.Token}");
        }

        var currency = Currency.ParseByValue<Currency>(request.Currency);

        var transaction = account.Deposit(request.Amount, currency, DateTime.UtcNow);

        return new DepositTransactionResponse(transaction.Id, transaction.Type.Value);
    }
}