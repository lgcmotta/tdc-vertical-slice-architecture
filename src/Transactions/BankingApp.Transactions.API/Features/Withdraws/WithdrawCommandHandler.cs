using BankingApp.Transactions.API.Infrastructure;
using BankingApp.Transactions.Domain.Exceptions;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace BankingApp.Transactions.API.Features.Withdraws;

public class WithdrawCommandHandler : IRequestHandler<WithdrawCommand, WithdrawTransactionResponse>
{
    private readonly AccountsDbContext _context;

    public WithdrawCommandHandler(AccountsDbContext context)
    {
        _context = context;
    }

    public async Task<WithdrawTransactionResponse> Handle(WithdrawCommand request, CancellationToken cancellationToken)
    {
        var account = await _context.Accounts
            .FirstOrDefaultAsync(account => account.Holder.Token == request.Token, cancellationToken)
            .ConfigureAwait(continueOnCapturedContext: false);

        if (account is null)
        {
            throw new AccountNotFoundException($"Account not found for token {request.Token}");
        }

        var transaction = account.Withdraw(request.Amount, DateTime.UtcNow);

        var amount = account.ConvertFromUSD(transaction.Value);

        var formattedAmount = amount.Format(account.Currency);

        return new WithdrawTransactionResponse(transaction.Id, transaction.Type.Value, account.Currency.Value, formattedAmount, amount);
    }
}