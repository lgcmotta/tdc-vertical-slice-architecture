using BankingApp.Transactions.API.Infrastructure;
using BankingApp.Transactions.Domain.Exceptions;
using MediatR;
using Microsoft.EntityFrameworkCore;

// ReSharper disable ClassNeverInstantiated.Global
namespace BankingApp.Transactions.API.Features.Withdraws;

public record CreateWithdrawCommand(string Token, decimal Amount) : IRequest<WithdrawTransactionResponse>;

public class CreateWithdrawCommandHandler : IRequestHandler<CreateWithdrawCommand, WithdrawTransactionResponse>
{
    private readonly AccountsDbContext _context;

    public CreateWithdrawCommandHandler(AccountsDbContext context)
    {
        _context = context;
    }

    public async Task<WithdrawTransactionResponse> Handle(CreateWithdrawCommand request, CancellationToken cancellationToken)
    {
        var account = await _context.Accounts
            .FirstOrDefaultAsync(account => account.Holder.Token == request.Token, cancellationToken)
            .ConfigureAwait(continueOnCapturedContext: false);

        if (account is null)
        {
            throw new AccountNotFoundException($"Account not found for token {request.Token}");
        }

        var transaction = account.Withdraw(request.Amount, DateTime.UtcNow);

        return new WithdrawTransactionResponse(transaction.Id, transaction.Type.Value);
    }
}