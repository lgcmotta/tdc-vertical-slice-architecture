using BankingApp.Transactions.API.Infrastructure;
using BankingApp.Transactions.Domain.Exceptions;
using BankingApp.Transactions.Domain.ValueObjects;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace BankingApp.Transactions.API.Features.Withdraw;

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
            .FirstOrDefaultAsync(account => account.Token == request.Token, cancellationToken)
            .ConfigureAwait(continueOnCapturedContext: false);

        if (account is null)
        {
            throw new AccountNotFoundException($"Account not found for token {request.Token}");
        }

        var transaction = account.Withdraw(request.Amount, DateTime.UtcNow);

        var amount = Money.ConvertFromUSD(transaction.USDValue, account.DisplayCurrency);

        var formattedAmount = amount.Format(account.DisplayCurrency);

        return new WithdrawTransactionResponse(transaction.Id, transaction.Type.Value, account.DisplayCurrency.Value, formattedAmount, amount);
    }
}